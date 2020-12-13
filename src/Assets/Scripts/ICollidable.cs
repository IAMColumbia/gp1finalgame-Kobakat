using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable
{
    Rect rect { get; set; }
    Texture2D texture { get; set; }
    Vector2 rectDim { get; set; }

}
