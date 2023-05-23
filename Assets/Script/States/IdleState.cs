using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : BaseState
{
    bool attack;
    public IdleState(P_Character _Character) : base(_Character)
    {
        Character = _Character;
    }

    public override void EnterState()
    {
        base.EnterState();
        _input = Vector2.zero;
        attack = false;
        _blockAttack.started += OnBlocked;
        _blockAttack.canceled += OnBlocked;
        _heavyAttack.performed += OnHeavyAttack;
    }

    private void OnHeavyAttack(InputAction.CallbackContext obj)
    {
        Character.Animator.SetTrigger("hAttack");
        Character.ChangeState(Character.HEAVYATTACKING);
    }

    private void OnBlocked(InputAction.CallbackContext obj)
    {
        switch (obj.phase)
        {
            case InputActionPhase.Started:
                isBlocked = true;
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                isBlocked = false;
                break;
        }
    }

    public override void InputManager()
    {
        base.InputManager();
        _input = _movement.ReadValue<Vector2>();
        if (_lightAttack.triggered)
        {
            attack = true;
        }
    }

    public override void LogicManager()
    {
        Character.Animator.SetFloat("speed", _input.magnitude, Character.PlayerSmoothDamp,Time.deltaTime);        
        if (_input.magnitude >= 0.1f) MovementUpdate(_playerSpeed);

        if (attack)
        {
            Character.Animator.SetTrigger("attack");
            Character.ChangeState(Character.ATTACKING);
        }

    }

    public override void ExitState()
    {
        base.ExitState();
        _blockAttack.started -= OnBlocked;
        _blockAttack.canceled -= OnBlocked;
        _heavyAttack.performed -= OnHeavyAttack;
    }

}
