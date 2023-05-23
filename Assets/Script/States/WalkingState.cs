using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : BaseState
{
    public WalkingState(P_Character _Character) : base(_Character)
    {
        Character = _Character;
    }

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void InputManager()
    {
        base.InputManager();
    }

    public override void LogicManager()
    {
        base.LogicManager();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
