using System;
using UnityEngine;

public class WinState : State
{
    float startTime;
    public WinState(Player sprite)
    {
        this.sprite = sprite;
        this.startTime = 0;
    }

    #region State Events
    public sealed override void OnStateEnter()
    {
        SetTimes();
        SetStates();
        base.OnStateEnter();
    }

    public sealed override void StateUpdate()
    {
        FlagLevelLoad();
        base.StateUpdate();
    }

    public sealed override void OnStateExit() { base.OnStateExit(); }

    #endregion

    #region Logic Functions

    void SetTimes()
    {
        startTime = Time.time;
    }

    void FlagLevelLoad()
    {
        if (Time.time > this.startTime + sprite.winTime)
            PlayerWon.Invoke();
    }

    void SetStates()
    {
        sprite.SetState(ref sprite.moveState, new GoalWalkState(sprite));
        sprite.SetState(ref sprite.groundState, new SlidingState(sprite));     
    }

    public static event Action PlayerWon;
    #endregion
}
