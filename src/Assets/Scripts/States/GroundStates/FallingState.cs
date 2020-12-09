using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : GroundState
{
    Player player;
    public FallingState(Player Player)
    {
        this.player = Player;
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
        player.yMoveDir -= player.gravityStrength * Time.deltaTime;
        player.yMoveDir = Mathf.Clamp(player.yMoveDir, -player.maxFallSpeed, 5000f);
    }

    #endregion
}
