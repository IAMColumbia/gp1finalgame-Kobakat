using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : AirborneState
{
    public SlidingState(Player sprite)
    {
        this.sprite = sprite;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        SetYMove();
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        PlayerController.Instance.Disable();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void SetYMove()
    {
        sprite.yMoveDir = -5.0f;
    }

    #endregion
}
