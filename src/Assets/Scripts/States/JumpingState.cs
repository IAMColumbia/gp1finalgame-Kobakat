using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player is currently holding the jump button
/// Applies an initial burst of speed coupled with a small increase over time
/// This state switches to the Rising state after the button is released or held long enough
/// </summary>
public class JumpingState : AirborneState
{
    public JumpingState(Player sprite)
    {
        this.sprite = sprite;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        ApplyDeltaJump();
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        SetUpwardsVelocity();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void SetUpwardsVelocity()
    {
        sprite.yMoveDir = sprite.jumpBurstStrength;
    }

    void ApplyDeltaJump()
    {
        sprite.yMoveDir += sprite.jumpDeltaStrength * Time.deltaTime;
    }

    #endregion
}
