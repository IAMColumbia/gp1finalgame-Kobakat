using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityEntity : MonoBehaviour
{
    public Entity entity;

    public virtual void Initialize()
    {
        entity = new Entity();

        entity.texture = GetComponent<SpriteRenderer>().sprite.texture;
        entity.position = this.transform.position;

        entity.rect = new Rect(
            entity.position.x,
            entity.position.y,
            entity.texture.width / Utility.pixelsPerUnit * transform.localScale.x,
            entity.texture.height / Utility.pixelsPerUnit * transform.localScale.x);

        entity.rectDim = new Vector2(entity.rect.width, entity.rect.height);
    }

    #region Debug
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(entity.rect);
    }

    void OnDrawGizmosSelected()
    {
        // Orange
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
        
        if(this is Player)
        {
            Player p = (Player)this;
            Debug.Log(p.groundState);
            Debug.Log(p.moveState);
            Debug.Log(p.gameState);
        }
        DrawRect(entity.rect);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(rect.position, new Vector3(rect.width, rect.height, 0));
    }
#endif
#endregion
}
