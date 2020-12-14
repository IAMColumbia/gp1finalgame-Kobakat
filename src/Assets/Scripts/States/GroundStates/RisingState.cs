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

    public sealed override void OnStateEnter() 
    {
        SetAnim();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void ApplyGravity()
    {
        player.mover.yMoveDir -= player.gravityStrength * Time.deltaTime;
        player.mover.yMoveDir = Mathf.Clamp(player.mover.yMoveDir, -player.maxFallSpeed, 5000f);
    }

    void CheckIfPlayerIsFallingAndChangeStateIfTheyAre()
    {
        if (player.mover.yMoveDir < 0)
            player.mover.SetState(ref player.groundState, new FallingState(player));
    }

    void SetAnim()
    {
        if(!(player.gameState is DyingState))
            player.anim.Play(player.jumpState);
    }
    #endregion
}
