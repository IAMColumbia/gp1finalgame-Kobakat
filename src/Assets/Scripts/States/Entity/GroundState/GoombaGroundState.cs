using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaGroundState : GroundState
{
    Goomba goomba;

    public GoombaGroundState(Goomba Goomba)
    {
        this.goomba = Goomba;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        goomba.mover.yMoveDir = 0;
        base.StateUpdate();
    }

    public sealed override void OnStateEnter()
    {
        goomba.mover.yMoveDir = 0;
        base.OnStateEnter();
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion
}
