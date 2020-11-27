using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovementState : State, IMoveState
{
    public MovementState(Player sprite) 
    {
        this.sprite = sprite;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        CheckIfPlayerIsTryingToTurn();

        Accelerate();
        UpdatePosition();

        UpdateRectAndCheckForCollisions();
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() { base.OnStateEnter(); }
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

    void Accelerate()
    {
        //Sprite is on the ground, they should have more control
        if(sprite.groundState is GroundedState)
            sprite.speed += sprite.xMoveDir * Time.deltaTime * sprite.acceleration;

        //Sprite is airborne, turning is harder
        else
            sprite.speed += (sprite.xMoveDir * Time.deltaTime * sprite.acceleration) / sprite.airborneMovementDamp;

        sprite.speed = Mathf.Clamp(sprite.speed, -sprite.maxSpeed, sprite.maxSpeed);
    }

    public void UpdateRectAndCheckForCollisions()
    {
        sprite.rect.position = sprite.transform.position;

        sprite.CheckForBlockCollisions();

        sprite.rect.position = sprite.transform.position;
    }

    /// <summary>
    /// If they are, set the state to turning
    /// </summary>
    void CheckIfPlayerIsTryingToTurn()
    {
        //Check if they're grounded
        if(sprite.groundState is GroundedState)
        {
            //Check if their input direction and movement direction conflict
            if (sprite.xMoveDir > 0 && sprite.speed < 0
            || sprite.xMoveDir < 0 && sprite.speed > 0)
            {
                //Set the state
                sprite.SetState(ref sprite.moveState, new TurningState(sprite));
            }
        }
    }
    #endregion
}
