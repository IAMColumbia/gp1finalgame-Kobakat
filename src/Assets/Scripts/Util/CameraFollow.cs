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
        this.target = level.playerTransform.transform;
    }

    void LateUpdate()
    {
        this.transform.position = new Vector3(
            target.position.x,
            0,
            -1);

        if (this.transform.position.x < 0)
        {
            this.transform.position = Vector3.back;
        }

        else if (this.transform.position.x > Utility.botRight.x - ((Camera.main.orthographicSize * Camera.main.aspect) / 2.0f))
        {
            this.transform.position = new Vector3(Utility.botRight.x - ((Camera.main.orthographicSize * Camera.main.aspect / 2.0f)), 0, -1);
        }

        Debug.Log(Utility.botRight.x - ((Camera.main.orthographicSize * Camera.main.aspect) / 2.0f));
    }
}
