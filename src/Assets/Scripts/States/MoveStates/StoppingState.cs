﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingState : State, IMoveState
{
    float startTime;
    float stopTime;
    public StoppingState(Player sprite) 
    {
        this.sprite = sprite;

        this.startTime = 0;
        this.stopTime = 0;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
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
    #endregion
}
