using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChunk : MonoBehaviour
{
    public List<Block> blocks { get; set; }
    public Rect rect { get; set; }
    public void Initialize(float width, float height)
    {
        this.rect = new Rect(this.transform.position, new Vector2(width, height));
        this.blocks = new List<Block>();
    }

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
