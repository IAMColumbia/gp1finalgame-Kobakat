using UnityEngine;
public abstract class State
{
    protected UnityMover mover;

    #region State Events
    public virtual void StateUpdate() { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    #endregion

}
