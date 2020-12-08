using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player is not necessarily falling but they have released the jump button
/// </summary>
public class RisingState : GroundState
{
    Player player;
    public RisingState(Player Player)
    {
        this.player = Player;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        ApplyGravity();
        CheckIfPlayerIsFallingAndChangeStateIfTheyAre();

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

    void CheckIfPlayerIsFallingAndChangeStateIfTheyAre()
    {
        if (player.yMoveDir < 0)
            player.SetState(ref player.groundState, new FallingState(player));
    }
    #endregion
}
