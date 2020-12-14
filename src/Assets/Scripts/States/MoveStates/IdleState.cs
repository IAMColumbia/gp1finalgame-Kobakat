using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MoveState
{
    Player player;
    public IdleState(Player Player)
    {
        player = Player;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        base.CheckForChunksCurrentlyIn(player);

        base.UpdatePosition(player);
        base.UpdateRectAndCheckForCollisions(player);
        base.CheckForCollisionWithOtherEntities(player);
    }

    public sealed override void OnStateEnter() 
    { 
        base.OnStateEnter();
        SetSpeed();
        SetAnim();
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    void SetAnim()
    {
        if(player.groundState is GroundedState)
            player.anim.Play(player.idleState);
    }

    void SetSpeed()
    {
        player.mover.speed = 0;
    }
}
