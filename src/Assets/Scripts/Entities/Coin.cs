using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : UnityEntity
{
    public sealed override void Initialize()
    {
        base.Initialize();

        //just push the coin back a bit
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            0.01f);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 240f * Time.deltaTime);
    }
}
