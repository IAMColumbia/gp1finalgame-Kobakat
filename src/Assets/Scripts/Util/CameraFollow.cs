using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;

    Level level;
    void Start()
    {
        this.level = FindObjectOfType<Level>().GetComponent<Level>();
        GetTarget();
    }

    void LateUpdate()
    {
        if(target)
        {
            this.transform.position = new Vector3(
            target.position.x,
            target.position.y,
            -1);

            if (this.transform.position.x < 0)
            {
                this.transform.position = new Vector3(
                    0,
                    this.transform.position.y,
                    -1);
            }

            else if (this.transform.position.x > Utility.botRight.x - Utility.camWidth)
            {
                this.transform.position = new Vector3(
                    Utility.botRight.x - Utility.camWidth,
                    this.transform.position.y,
                    -1);
            }

            if (this.transform.position.y < 0)
            {
                this.transform.position = new Vector3(
                    this.transform.position.x,
                    0,
                    -1);
            }
        }      
    }

    public void GetTarget()
    {
        this.target = level.playerTransform.transform;
    }
}
