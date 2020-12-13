using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blocks are simply static sprites
/// More precisely, their rect is static and will not move with the block
/// Transformations can still be performed but they will only be visible
/// This is useful for hitting blocks with mario's head so the block can still "animate" getting hit
/// </summary>

public interface IBlock
{
    Vector3 position { get; set; }
}
public class Block : ICollidable, IBlock
{
    public Texture2D texture { get; set; }

    public Rect rect { get; set; }
    public Vector3 position { get; set; }   
    public Vector2 rectDim { get; set; }

    public Block()
    {           
        this.texture = null;

        this.rect = new Rect();

        this.position = Vector3.zero;
        this.rectDim = Vector2.zero;
    }

}
