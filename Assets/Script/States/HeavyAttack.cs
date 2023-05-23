using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttack : BaseState
{
    float timePassed;
    float clipLength;
    float clipSpeed;

    public HeavyAttack(P_Character _Character) : base(_Character)
    {
        Character = _Character;
    }
    public override void EnterState() {
        base.EnterState();
        timePassed = 0;
        Character.Animator.applyRootMotion = true;
        Character.Animator.SetTrigger("hAttack");
        Character.Animator.SetFloat("speed", 0);        
    }
    public override void InputManager() {
        base.InputManager();
    }
    public override void LogicManager() {
        base.LogicManager();
        HeavyAttackLogic();
    }
    public override void ExitState() {
        base.ExitState();
        Character.Animator.applyRootMotion = false;
    }

    void HeavyAttackLogic()
    {
        timePassed += Time.deltaTime;
        clipLength = Character.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        clipSpeed = Character.Animator.GetCurrentAnimatorStateInfo(0).speed;
        if (timePassed >= clipLength / clipSpeed)
        {
            Character.ChangeState(Character.IDLE);
            Character.Animator.SetTrigger("move");
        }
    }
}
