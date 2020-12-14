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

    void SetAnim()
    {
        if(!(player.gameState is DyingState))
            player.anim.Play(player.jumpState);
    }

    #endregion
}
