using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;
    public AttackState(P_Character _Character) : base(_Character)
    {
        Character = _Character;
    }
    public override void EnterState() {
        base.EnterState();
        attack = false;
        isBlocked = false;
        timePassed = 0;
        Character.Animator.applyRootMotion = true;
        Character.Animator.SetTrigger("attack");
        Character.Animator.SetFloat("speed", 0);
    }
    public override void InputManager() {
        base.InputManager();

        if (_lightAttack.triggered)
        {
            attack = true;
        }
    }
    public override void LogicManager() {
        base.LogicManager();
        LightAttackLogic();
        Character.Animator.SetBool("block", isBlocked);

    }
    public override void ExitState() {
        base.ExitState();
        Character.Animator.applyRootMotion = false;
    }

    public void LightAttackLogic()
    {
        timePassed += Character.PlayerAttackSpeedModifier + Time.deltaTime;
        clipLength = Character.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        clipSpeed = Character.Animator.GetCurrentAnimatorStateInfo(0).speed;

        if (timePassed >= clipLength / clipSpeed && attack)
        {
            Character.ChangeState(Character.ATTACKING);
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            Character.ChangeState(Character.IDLE);
            Character.Animator.SetTrigger("move");
        }

    }
}
