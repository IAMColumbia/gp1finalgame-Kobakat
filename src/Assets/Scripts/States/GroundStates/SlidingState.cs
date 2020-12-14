using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : GroundState
{
    Player player;
    public SlidingState(Player Player)
    {
        this.player = Player;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        SetYMove();
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        PlayerController.Instance.Disable();
        SetAnim();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void SetYMove()
    {
        player.mover.yMoveDir = -5.0f;
    }

    void SetAnim()
    {
        player.anim.Play(player.grabState);
    }

    #endregion
}
