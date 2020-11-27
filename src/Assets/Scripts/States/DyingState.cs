using System;
using UnityEngine;

public class DyingState : State
{
    float startTime;
    public DyingState(Player sprite)
    {
        this.sprite = sprite;
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
        sprite.transform.position = new Vector3(
            sprite.transform.position.x,
            sprite.transform.position.y + sprite.yMoveDir * Time.deltaTime
            -.01f); //Pull the player character in front of all other sprites
    }

    void SetInitialYDirection()
    {
        //hack hard coded
        sprite.yMoveDir = sprite.jumpBurstStrength * 2;
    }

    void ApplyGravity()
    {
        sprite.yMoveDir -= sprite.gravityStrength * Time.deltaTime;
        sprite.yMoveDir = Mathf.Clamp(sprite.yMoveDir, -sprite.maxFallSpeed, 5000f);
    }

    void FlagLevelLoad()
    {
        if(Time.time > this.startTime + sprite.deathTime)
            PlayerDied.Invoke();
    }

    public static event Action PlayerDied;
    #endregion
}
