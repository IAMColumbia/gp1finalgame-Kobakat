using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player is not necessarily falling but they have released the jump button
/// </summary>
public class RisingState : AirborneState
{
    public RisingState(Player sprite)
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

    public sealed override void OnStateEnter() { base.OnStateEnter(); }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions
    void ApplyGravity()
    {
        sprite.yMoveDir -= sprite.gravityStrength * Time.deltaTime;
    }

    void CheckIfPlayerIsFallingAndChangeStateIfTheyAre()
    {
        if (sprite.yMoveDir < 0)
            sprite.SetState(ref sprite.groundState, new FallingState(sprite));
    }
    #endregion
}
