using UnityEngine;
using System.Collections;

public class guncontrolsfromtut : MonoBehaviour
{
    public GameObject Bullet_Emitter;

    public GameObject Bullet;

    public float Bullet_Up_Force;

    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position + new Vector3(0.0f, 1.0f, 0.0f), Bullet_Emitter.transform.rotation) as GameObject;

            Temporary_Bullet_Handler.transform.Rotate(Vector3.forward * 110);
            Temporary_Bullet_Handler.transform.Rotate(Vector3.right * 00);
            Temporary_Bullet_Handler.transform.Rotate(Vector3.up * 00);

            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

            Temporary_RigidBody.AddForce(transform.up * Bullet_Up_Force);

            Destroy(Temporary_Bullet_Handler, 2.0f);
        }
    }
}