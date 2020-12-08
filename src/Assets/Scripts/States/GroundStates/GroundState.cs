using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundState : State
{
    #region State Events
    public override void StateUpdate() { base.StateUpdate(); }
    public override void OnStateEnter() { base.OnStateEnter(); }
    public override void OnStateExit() { base.OnStateExit(); }

    #endregion
}

