﻿using System;
using UnityEngine;

public class WinState : GameState
{
    Player player;
    float startTime;
    public WinState(Player Player)
    {
        this.player = Player;
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
        if (Time.time > this.startTime + player.winTime)
            PlayerWon.Invoke();
    }

    void SetStates()
    {
        player.SetState(ref player.moveState, new GoalWalkState(player));
        player.SetState(ref player.groundState, new SlidingState(player));     
    }

    public static event Action PlayerWon;
    #endregion
}
