using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    Vector3 position { get; set; } 
}

public class Entity : ICollidable, IEntity
{  
    public Rect rect { get; set; }
    public Texture2D texture { get; set; }

    public Vector3 position { get; set; }
    public Vector2 rectDim { get; set; } //Just used for cleaner code

    public Entity()
    {
        this.rect = new Rect();
        
        this.texture = null;

        this.position = Vector3.zero;
        this.rectDim = Vector2.zero;
    }
}
