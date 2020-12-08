using System;
using UnityEngine;

public class DyingState : GameState
{
    Player player;
    float startTime;
    public DyingState(Player Player)
    {
        this.player = Player;
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
        UpdatePosition();
        ApplyGravity();

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

    void UpdatePosition()
    {
        player.transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + player.yMoveDir * Time.deltaTime
            -.01f); //Pull the player character in front of all other sprites
    }

    void SetInitialYDirection()
    {
        //hack hard coded
        player.yMoveDir = player.jumpBurstStrength * 2;
    }

    void ApplyGravity()
    {
        player.yMoveDir -= player.gravityStrength * Time.deltaTime;
        player.yMoveDir = Mathf.Clamp(player.yMoveDir, -player.maxFallSpeed, 5000f);
    }

    void FlagLevelLoad()
    {
        if(Time.time > this.startTime + player.deathTime)
            PlayerDied.Invoke();
    }


    public static event Action PlayerDied;
    #endregion
}
