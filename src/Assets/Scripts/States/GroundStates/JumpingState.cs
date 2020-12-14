﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player is currently holding the jump button
/// Applies an initial burst of speed coupled with a small increase over time
/// This state switches to the Rising state after the button is released or held long enough
/// </summary>
public class JumpingState : GroundState
{
    Player player;
    public JumpingState(Player Player)
    {
        this.player = Player;
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
        SetAnim();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void SetUpwardsVelocity()
    {
        player.mover.yMoveDir = player.jumpBurstStrength;
    }

    void ApplyDeltaJump()
    {
        player.mover.yMoveDir += player.jumpDeltaStrength * Time.deltaTime;
    }

    void SetAnim()
    {
        player.anim.Play(player.jumpState);
    }

    #endregion
}
