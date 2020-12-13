using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningState : MoveState
{
    float startTime;
    float stopTime;
    const float epsilon = 0.1f;

    Player player;
    public TurningState(Player Player)
    {
        this.player = Player;
        startTime = 0;
        stopTime = 0;
    }

    #region State Events
    
    public sealed override void StateUpdate()
    {
        SetStateToMovementAfterTurningFinishes();
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

    void SetStateToMovementAfterTurningFinishes()
    {
        //Check if they're grounded
        if (player.groundState is GroundedState)
        {
            //Check if their input direction and movement direction conflict
            if (player.mover.xMoveDir > 0 && player.mover.speed >= 0 - epsilon
            || player.mover.xMoveDir < 0 && player.mover.speed <= 0 + epsilon)
            {
                //Set the state
                player.mover.SetState(ref player.moveState, new MovementState(player));
            }
        }

        //If they're not grounded they either jumped or fell, switch move states
        else 
        {
            player.mover.SetState(ref player.moveState, new MovementState(player));
        }
    }

    #endregion
}
