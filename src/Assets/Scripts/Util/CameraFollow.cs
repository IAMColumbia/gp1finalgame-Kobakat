using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if(target && target.GetComponent<Player>().gameState is PlayState)
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

            else if (this.transform.position.x > Utility.topRight.x - Utility.camWidth)
            {
                this.transform.position = new Vector3(
                    Utility.topRight.x - Utility.camWidth,
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
}
