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
        PlayerJustDied.Invoke();

        SetTimes();
        SetInitialYDirection();
        SetAnim();
        PlayAudio();
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
        player.entity.position = new Vector3(
            player.entity.position.x,
            player.entity.position.y + player.mover.yMoveDir * Time.deltaTime,
            -0.1f); //Pull the player character in front of all other sprites
        
        player.transform.position = player.entity.position;
    }

    void SetInitialYDirection()
    {
        player.mover.yMoveDir = player.jumpBurstStrength * 3;
    }

    void ApplyGravity()
    {
        player.mover.yMoveDir -= player.gravityStrength * Time.deltaTime;
        player.mover.yMoveDir = Mathf.Clamp(player.mover.yMoveDir, -player.maxFallSpeed, 5000f);
    }

    void CheckForPauseComplete()
    {
        if (Time.time > this.startTime + player.deathPauseTime)
        {
            donePausing = true;
            PlayerPauseComplete.Invoke();
        }           
    }

    void FlagLevelLoad()
    {
        if(Time.time > this.startTime + player.deathTime)
            PlayerDied.Invoke();
    }

    void SetAnim()
    {
        player.anim.Play(player.dyingState);
    }

    void PlayAudio()
    {
        player.SFX.PlayClip(player.SFX.dieClip);
    }

    public static event Action PlayerDied;
    public static event Action PlayerPauseComplete;
    public static event Action PlayerJustDied;
    #endregion
}
