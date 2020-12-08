using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blocks are simply static sprites
/// More precisely, their rect is static and will not move with the block
/// Transformations can still be performed but they will only be visible
/// This is useful for hitting blocks with mario's head so the block can still "animate" getting hit
/// </summary>

public interface ICollidable
{
    Rect rect { get; set; }
    Texture2D texture { get; set; }
    abstract void HitTop();
    abstract void HitSide();
    abstract void HitBottom();
}

public abstract class Block : MonoBehaviour, ICollidable
{
    public Rect rect { get; set; }
    public Texture2D texture { get; set; }

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
    public abstract void HitTop();
    public abstract void HitSide();
    public abstract void HitBottom();

    #region Debug
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(rect);
    }

    void OnDrawGizmosSelected()
    {
        // Orange
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
        DrawRect(rect);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(rect.position, new Vector3(rect.size.x, rect.size.y, 0.1f));
    }
#endif
    #endregion
}
