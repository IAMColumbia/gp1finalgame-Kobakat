using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBlock : MonoBehaviour
{
    public Block block;
    
    public virtual void Initialize()
    {
        block = new Block();

        block.texture = this.GetComponent<SpriteRenderer>().sprite.texture;
        
        block.position = this.transform.position;

        block.rect = new Rect(
            block.position.x,
            block.position.y,
            block.texture.width / Utility.pixelsPerUnit,
            block.texture.height / Utility.pixelsPerUnit);          
    }

    //What should the block do when it gets hit from any particular side
    //Unlike entities, collisions have no basic behavior so they don't need to call anything from Block
    public virtual void HitBottom() { }
    public virtual void HitSide() { }
    public virtual void HitTop() { }

    #region Debug
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(block.rect);
    }

    void OnDrawGizmosSelected()
    {
        // Orange
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
        DrawRect(block.rect);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(rect.position, new Vector3(rect.size.x, rect.size.y, 0.1f));
    }
#endif
    #endregion

}
