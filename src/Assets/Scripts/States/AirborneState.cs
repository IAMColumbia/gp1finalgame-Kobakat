using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class only exists as an intermediary step for Falling and Jumping states
/// This makes tracking a if the player has already double jumped much easier
/// </summary>
public abstract class AirborneState : State
{
    #region State Events
    public override void StateUpdate() { base.StateUpdate(); }
    public override void OnStateEnter() { base.OnStateEnter(); }
    public override void OnStateExit() { base.OnStateExit(); }

    #endregion
}
