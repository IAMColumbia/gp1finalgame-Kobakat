using System;
using UnityEngine;

public class DyingState : GameState
{
    Player player;
    float startTime;
    bool donePausing;

    //HACK ugly fix to the player checking for collisions once more before truly entering the state
    bool waitedUntilNextGameLoop;
    public DyingState(Player Player)
    {
        this.player = Player;
        this.startTime = 0;
        this.donePausing = false;
        this.waitedUntilNextGameLoop = false;        
    }

    #region State Events
    public sealed override void OnStateEnter()
    {
        SetTimes();
        SetInitialYDirection();
        base.OnStateEnter();
    }

    public sealed override void StateUpdate()
    {
        if(!waitedUntilNextGameLoop)
        {
            this.OnStateEnter();
            waitedUntilNextGameLoop = true;
        }

        //Makes mario stay still for a second before dropping down. Comedic effect
        if(donePausing)
        {
            UpdatePosition();
            ApplyGravity();

            FlagLevelLoad();
            base.StateUpdate();
        }
        else
        {
            CheckForPauseComplete();
        }    
    }

    public sealed override void OnStateExit() { base.OnStateExit(); }

    #endregion

    #region Logic Functions

    void SetTimes()
    {
        startTime = Time.time;
    }

    void UpdatePosition()
    {
        player.transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + player.yMoveDir * Time.deltaTime,
            -0.1f); //Pull the player character in front of all other sprites
    }

    void SetInitialYDirection()
    {
        player.yMoveDir = player.jumpBurstStrength * 2;
    }

    void ApplyGravity()
    {
        player.yMoveDir -= player.gravityStrength * Time.deltaTime;
        player.yMoveDir = Mathf.Clamp(player.yMoveDir, -player.maxFallSpeed, 5000f);
    }

    void CheckForPauseComplete()
    {
        if (Time.time > this.startTime + player.deathPauseTime)
            donePausing = true;
    }

    void FlagLevelLoad()
    {
        if(Time.time > this.startTime + player.deathTime)
            PlayerDied.Invoke();
    }


    public static event Action PlayerDied;
    #endregion
}
