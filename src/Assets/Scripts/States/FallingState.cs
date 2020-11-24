using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirborneState
{
    public FallingState(Player sprite)
    {
        this.sprite = sprite;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        ApplyGravity();
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() { base.OnStateEnter(); }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void ApplyGravity()
    {
        sprite.yMoveDir -= sprite.gravityStrength;
    }

    #endregion
}
