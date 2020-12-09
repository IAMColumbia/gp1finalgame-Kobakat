using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingState : MoveState
{
    Player player;

    float startTime;
    float stopTime;
    public StoppingState(Player Player) 
    {
        player = Player;

        this.startTime = 0;
        this.stopTime = 0;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        SlowSpeedDown();

        base.CheckForChunksCurrentlyIn(player);
        base.UpdatePosition(player);
        base.UpdateRectAndCheckForCollisions(player);
        base.CheckForCollisionWithOtherEntities(player);

    }

    public sealed override void OnStateEnter() 
    {
        SetStopTimeBasedOnCurrentSpeed();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions

    void SlowSpeedDown()
    {
        player.speed = Mathf.Lerp(
            player.speed,
            0,
            (Time.time - startTime) / stopTime);
    }

    void SetStopTimeBasedOnCurrentSpeed()
    {
        this.startTime = Time.time;
        this.stopTime = Mathf.Abs((player.speed / (player.maxSpeed * player.frictionStrength)));
    }
    #endregion
}
