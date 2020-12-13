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
            entity.texture.width / Utility.pixelsPerUnit,
            entity.texture.height / Utility.pixelsPerUnit);

        entity.rectDim = new Vector2(entity.rect.width, entity.rect.height);
    }
}
