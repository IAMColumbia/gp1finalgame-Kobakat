using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBlockChunk : MonoBehaviour
{
    public BlockChunk chunk;
    public List<UnityBlock> unityBlocks;
    public void Initialize(float width, float height)
    {
        chunk = new BlockChunk();

        chunk.position = this.transform.position;
        chunk.rect = new Rect(chunk.position, new Vector2(width, height));
        unityBlocks = new List<UnityBlock>();
    }

    #region Debug
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(chunk.rect);
    }

    void OnDrawGizmosSelected()
    {
        // Orange
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
        DrawRect(chunk.rect);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(rect.position, new Vector3(rect.size.x, rect.size.y, 0.1f));
    }
#endif
    #endregion
}
