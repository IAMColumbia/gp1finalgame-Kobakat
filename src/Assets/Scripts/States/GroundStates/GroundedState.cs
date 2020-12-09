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
        player.yMoveDir = 0;
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        player.yMoveDir = 0;
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion
}
