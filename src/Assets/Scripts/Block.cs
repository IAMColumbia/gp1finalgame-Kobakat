﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Rect rect;
    public Sprite sprite { get; set; }
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>().sprite;
        rect = new Rect(this.transform.position, new Vector2(sprite.texture.width / 100.0f, sprite.texture.height / 100.0f));
    }

    #region Debug
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
    #endregion
}
