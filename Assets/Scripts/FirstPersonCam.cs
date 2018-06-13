using UnityEngine;
using System.Collections;

public class FirstPersonCam : MonoBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }
}
/*
public float horizontalSpeed = 2.0F;
public float verticalSpeed = 2.0F;
void Update()
{
    float h = horizontalSpeed * Input.GetAxis("Mouse X");
    float v = verticalSpeed * Input.GetAxis("Mouse Y");
    //transform.Rotate(v *-2, h, 0);
}
}
*/
