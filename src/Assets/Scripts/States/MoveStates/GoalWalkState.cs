using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWalkState : MoveState
{
    bool hasHitBlock;
    Player player;

    public GoalWalkState(Player Player)
    {
        this.player = Player;
        this.hasHitBlock = false;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        base.CheckForChunksCurrentlyIn(player);
        base.UpdatePosition(player);
        base.UpdateRectAndCheckForCollisions(player);

        if(!hasHitBlock)
        {
            if (CheckIfBlockHit())
            {
                hasHitBlock = true;
                this.SetWalkSpeed();
            }

        }
    }

    public sealed override void OnStateEnter() 
    {
        KillSpeed();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions

    void KillSpeed()
    {
        player.xMoveDir = 0;
        player.speed = 0;
    }

    bool CheckIfBlockHit()
    {
        return (player.groundState is GroundedState);
    }
    void SetWalkSpeed()
    {
        player.xMoveDir = 1;
        player.speed = 5.0f;
    }

    #endregion
}
