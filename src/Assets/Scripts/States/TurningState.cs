using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningState : State, IMoveState
{
    float startTime;
    float stopTime;

    public TurningState(Player sprite)
    {
        this.sprite = sprite;
        startTime = 0;
        stopTime = 0;
    }

    #region State Events
    
    public sealed override void StateUpdate()
    {
        SetStateToMovementAfterTurningFinishes();

        SlowSpeedDown();
        UpdatePosition();

        UpdateRectAndCheckForCollisions();
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        SetStopTimeBasedOnCurrentSpeed();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions

    public void UpdatePosition()
    {
        sprite.transform.position = new Vector3(
            sprite.transform.position.x + (sprite.speed * Time.deltaTime),
            sprite.transform.position.y + (sprite.yMoveDir * Time.deltaTime),
            sprite.transform.position.z);
    }

    public void UpdateRectAndCheckForCollisions()
    {
        sprite.rect.position = sprite.transform.position;

        sprite.CheckForBlockCollisions();

        sprite.rect.position = sprite.transform.position;
    }

    void SlowSpeedDown()
    {
        sprite.speed = Mathf.Lerp(
            sprite.speed,
            0,
            (Time.time - startTime) / stopTime);
    }

    void SetStopTimeBasedOnCurrentSpeed()
    {
        this.startTime = Time.time;
        this.stopTime = Mathf.Abs((sprite.speed / (sprite.maxSpeed * sprite.frictionStrength)));
    }

    void SetStateToMovementAfterTurningFinishes()
    {
        //Check if they're grounded
        if (sprite.moveState is GroundedState)
        {
            //Check if their input direction and movement direction conflict
            if (sprite.xMoveDir > 0 && sprite.speed > 0
            || sprite.xMoveDir < 0 && sprite.speed < 0)
            {
                //Set the state
                sprite.SetState(ref sprite.moveState, new TurningState(sprite));
            }
        }
    }

    #endregion
}
