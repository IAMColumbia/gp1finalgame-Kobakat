using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, ICollidable
{  
    public Rect rect { get; set; }
    public Texture2D texture { get; set; }

    public List<BlockChunk> chunksToCheckCollisionFor;
    public List<Entity> entitiesToCheckCollisionFor;
    public List<BlockChunk> chunksCurrentlyIn;

    public float speed { get; set; }
    public float xMoveDir { get; set; }
    public float yMoveDir { get; set; }
    public int blockTouchCount { get; set; }

    public virtual void Initialize(List<BlockChunk> chunks, List<Entity> entities)
    {
        this.chunksToCheckCollisionFor = chunks;
        this.entitiesToCheckCollisionFor = entities;
        this.chunksCurrentlyIn = new List<BlockChunk>();
    }

    protected virtual void Awake()
    {
        texture = this.GetComponent<SpriteRenderer>().sprite.texture;
        rect = new Rect(
            this.transform.position,
            new Vector2(
            texture.width / Utility.pixelsPerUnit,
            texture.height / Utility.pixelsPerUnit));
    }

    
    //What to do when a player (or maybe even an enemy) hits a block from a specified side
    public virtual void HitTop(Block b) { }
    public virtual void HitSide(Block b, float side) { }
    public virtual void HitBottom(Block b) { }

    public virtual void CheckForFall() { }

    public void SetState(ref State whichState, State type)
    {
        whichState.OnStateExit();
        whichState = type;
        whichState.OnStateEnter();
    }
}
