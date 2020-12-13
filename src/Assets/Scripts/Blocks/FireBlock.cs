using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This entire class is very designed and strongly bound to unity
/// It also parents entities under the block manager. While the entiymanger is still doing all the work,
/// the whole design of this is still a mess
/// </summary>
public class FireBlock : UnityBlock
{
    [SerializeField] GameObject Fire;
    [SerializeField] int fireCount = 6;
    [SerializeField] float distance = 1;
    [SerializeField] float speed = 30;
    [SerializeField] Transform rotationAxis;

    public List<Fire> fires { get; set; }
       
    public sealed override void Initialize()
    {
        base.Initialize();
        
        LoadFires();
    }

    void LoadFires()
    {
        fires = new List<Fire>();

        for (int i = 0; i < fireCount; i++)
        {
            Vector3 pos = new Vector3(
                this.block.position.x,
                this.block.position.y + (i * distance),
                -0.05f);

            fires.Add(Instantiate(Fire, pos, rotationAxis.rotation, rotationAxis.transform).GetComponent<Fire>());
        }
    }

    void Update()
    {
        rotationAxis.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
