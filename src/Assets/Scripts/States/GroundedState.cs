using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
    public GroundedState(Player sprite)
    {
        this.sprite = sprite;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        sprite.yMoveDir = 0;
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        sprite.yMoveDir = 0;
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion
}
