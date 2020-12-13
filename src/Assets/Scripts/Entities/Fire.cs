using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heavily designed, very bad.
/// </summary>
public class Fire : UnityEntity
{
    [SerializeField] float speed = 30;
    public sealed override void Initialize()
    {
        base.Initialize();

        entity.rectDim = new Vector2(entity.rectDim.x * transform.localScale.x, entity.rectDim.y * transform.localScale.y);
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);

        //Could apply a rotation matrix to the rect but the sprite is almost a true square so it doesn't really matter
        entity.rect = new Rect(
            transform.position,
            entity.rectDim);

    }
}
