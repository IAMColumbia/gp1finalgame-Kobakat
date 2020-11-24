using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : AirborneState
{
    public JumpingState(Player sprite)
    {
        this.sprite = sprite;
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
        SetUpwardsVelocity();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void SetUpwardsVelocity()
    {
        sprite.yMoveDir = sprite.jumpStrength;
    }

    void ApplyGravity()
    {
        sprite.yMoveDir -= sprite.gravityStrength;
    }

    void CheckIfPlayerIsFallingAndChangeStateIfTheyAre()
    {
        if (sprite.yMoveDir < 0)
            sprite.SetState(ref sprite.groundState, new FallingState(sprite));
    }
    #endregion
}
