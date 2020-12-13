using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaFallingState : GroundState
{
    Goomba goomba;

    public GoombaFallingState(Goomba Goomba)
    {
        this.goomba = Goomba;
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
        goomba.mover.yMoveDir -= goomba.gravityStrength * Time.deltaTime;
        goomba.mover.yMoveDir = Mathf.Clamp(goomba.mover.yMoveDir, -goomba.maxFallSpeed, 5000f);
    }

    #endregion
}
