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

        CheckIfStopped();
    }

    public sealed override void OnStateEnter() 
    {
        SetStopTimeBasedOnCurrentSpeed();
        SetAnim();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions

    void SlowSpeedDown()
    {
        player.mover.speed = Mathf.Lerp(
            player.mover.speed,
            0,
            (Time.time - startTime) / stopTime);
    }

    void SetStopTimeBasedOnCurrentSpeed()
    {
        this.startTime = Time.time;
        this.stopTime = Mathf.Abs((player.mover.speed / (player.maxSpeed * player.frictionStrength)));
    }

    void CheckIfStopped()
    {
        if(Mathf.Abs(player.mover.speed) <= Utility.epsilon)
        {
            player.mover.SetState(ref player.moveState, new IdleState(player));
        }
    }

    void SetAnim()
    {
        if(player.groundState is GroundedState)
            player.anim.Play(player.walkState);
    }
    #endregion
}
