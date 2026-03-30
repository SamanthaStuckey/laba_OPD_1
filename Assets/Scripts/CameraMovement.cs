using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    float mouseWheel;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + new Vector3(0, 0.03f, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + new Vector3(0, -0.03f, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + new Vector3(-0.03f, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + new Vector3(0.03f, 0, 0);
        }

        mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel") * -1;

        if(Camera.main.orthographicSize > 1)
        {
            Camera.main.orthographicSize += mouseWheel;
        }
        else if(Camera.main.orthographicSize < 15)
        {
            Camera.main.orthographicSize += mouseWheel;
        }

    }
}
