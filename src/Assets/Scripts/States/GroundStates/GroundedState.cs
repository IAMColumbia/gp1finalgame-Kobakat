using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : GroundState
{
    Player player;
    public GroundedState(Player Player)
    {
        this.player = Player;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        player.mover.yMoveDir = 0;
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        player.mover.yMoveDir = 0;
        SetAnim();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    void SetAnim()
    {
        if (player.moveState is IdleState)
            player.anim.Play(player.idleState);
        else
            player.anim.Play(player.walkState);
    }
}
