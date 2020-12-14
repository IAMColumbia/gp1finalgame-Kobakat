using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I am too lazy to put all the methods in this interface.
/// </summary>
public interface IMover
{
    Entity entity { get; set; }

    List<BlockChunk> chunksToCheckCollisionFor { get; set; }
    List<BlockChunk> chunksCurrentlyIn { get; set; }
    List<Entity> entitiesToCheckCollisionFor { get; set; }

    int unitTouchCount { get; set; } //Is the mover currently touching anything else?
    int blockTouchCount { get; set; }
    int entityTouchCount { get; set; }

    float speed { get; set; }
    float xMoveDir { get; set; }
    float yMoveDir { get; set; }
}

/// <summary>
/// An entity that moves
/// </summary>
public class Mover : IMover
{
    public Entity entity { get; set; }

    public List<BlockChunk> chunksToCheckCollisionFor { get; set; }
    public List<BlockChunk> chunksCurrentlyIn { get; set; }
    public List<Entity> entitiesToCheckCollisionFor { get; set; }

    public int unitTouchCount { get; set; }
    public int blockTouchCount { get; set; }
    public int entityTouchCount { get; set; }
    public float speed { get; set; }
    public float xMoveDir { get; set; }
    public float yMoveDir { get; set; }

    public Mover(Entity Entity, List<BlockChunk> Chunks, List<Entity> Entities)
    {
        this.entity = Entity;

        this.chunksToCheckCollisionFor = Chunks;
        this.entitiesToCheckCollisionFor = Entities;
        this.chunksCurrentlyIn = new List<BlockChunk>();

        this.unitTouchCount = 0;
    }

    //Note that these collision events can be overriden by each mover subtype
    //They can play by these "normal" rules or not at all
    #region Block Collision Event
    
    //Set position directly ontop of the block
    public void HitTop(Block b) 
    {
        entity.position = new Vector3(
            entity.position.x,
            b.rect.position.y + (b.rect.height / 2.0f) + (entity.rectDim.y / 2.0f),
            0);
    }
    
    //Set position on directly on the side of the block
    public void HitSide(Block b, float side) 
    {
        //We hit the left side of the block
        if (side < 0)
        {
            entity.position = new Vector3(
                b.rect.position.x - (b.rect.width / 2.0f) - (entity.rectDim.x / 2.0f) - Utility.epsilon,
                entity.position.y,
                0);
        }

        //We hit the right side of the block
        else
        {
            entity.position = new Vector3(
                b.rect.position.x + (b.rect.width / 2.0f) + (entity.rectDim.x / 2.0f) + Utility.epsilon,
                entity.position.y,
                0);
        }
    }

    //Set position directly below the block
    public void HitBottom(Block b) 
    {
        entity.position = new Vector3(
                entity.position.x,
                b.rect.position.y - (b.rect.height / 2.0f) - (entity.rectDim.y / 2.0f),
                0);
    }

    #endregion

    #region Entity Collision Event
    //Snap position to the top of the entity
    public void HitTop(Entity e) 
    {
        entity.position = new Vector3(
            entity.position.x,
            e.position.y + (e.rectDim.y / 2.0f) + (entity.rectDim.y / 2.0f),
            0);

        entityTouchCount++;
    }

    //Snap to the side of the entity
    public void HitSide(Entity e, float side) 
    {
        //We hit the left side of the entity
        if (side < 0)
        {
            entity.position = new Vector3(
                e.position.x - (entity.rectDim.x / 2.0f) - (entity.rectDim.x / 2.0f) - (Utility.epsilon * 2),
                entity.position.y,
                0);
        }

        //We hit the right side of the entity
        else
        {
            entity.position = new Vector3(
                e.position.x + (e.rectDim.x / 2.0f) + (entity.rectDim.x / 2.0f) + (Utility.epsilon * 2) ,
                entity.position.y,
                0);
        }

    }

    public void HitBottom(Entity e) { } //Do nothing for now

    //No hit bottom event but adding one is easy and follows OC principle

    #endregion

    /// <summary>
    /// Checks if the mover is touching anything
    /// If its not, we can assume it should enter a falling state if it isn't already in one
    /// The implmentation will be left up to the specific mover
    /// </summary>
    /// <returns>Is the entity on top of a block</returns>
    public bool CheckForFall() 
    {
        if (this.unitTouchCount == 0)
            return true;

        return false;
    }
    
    /// <summary>
    /// Checks if the entity is going out of bounds on the X axis
    /// </summary>
    /// <returns>Did the entity move off screen on the x axis</returns>
    public bool WentOutOfBoundsSideways()
    {
        //Mover is past the left screen bound, snap them and their rect back to the screen edge
        if (entity.position.x < Utility.botLeft.x + (entity.rectDim.x / 2.0f))
        {
            entity.position = new Vector3(
                Utility.botLeft.x + (entity.rectDim.x / 2.0f) + Utility.epsilon,
                entity.position.y,
                0);

            entity.rect = new Rect(
                entity.position,
                entity.rectDim);

            return true;
        }

        //Mover is past the right screen bound, snap them and their rect back to the screen edge
        else if (entity.position.x > Utility.topRight.x - (entity.rectDim.x / 2.0f))
        {
            entity.position = new Vector3(
                Utility.topRight.x - (entity.rectDim.x / 2.0f),
                entity.position.y,
                0);

            entity.rect = new Rect(
                entity.position,
                entity.rectDim);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the mover went out of bounds on the y axis
    /// </summary>
    /// <returns>Did the entity fall below the screen</returns>
    public bool WentOutOfBoundsDown()
    {
        //Mover fell off the bottom of the level. Die!
        if (entity.position.y < Utility.botLeft.y - entity.rectDim.y / 2.0f)
            return true;

        return false;
    }
  
    /// <summary>
    /// Some movers may decide to use a state pattern to decide their game logic
    /// This transitions the states used
    /// </summary>
    /// <param name="whichState">Which state belonging to the entity is being changed</param>
    /// <param name="newState">What type of state to replace the current state with</param>
    public void SetState(ref State whichState, State newState)
    {
        whichState.OnStateExit();
        whichState = newState;
        whichState.OnStateEnter();
    }
}
