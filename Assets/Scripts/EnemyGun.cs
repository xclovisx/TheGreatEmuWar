using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
    public GameObject Bullet_Emitter;

    public GameObject Bullet;

    public float Bullet_Up_Force;

    private float time = 0.0f;
    private float timeBegin = 10.0f;
    public float interpolationPeriod = 5.0f;

    void Start()
    {
    }
    void Update()
    {
        timeBegin += Time.deltaTime;

        if (timeBegin >= 10.0f)
        {
            time += Time.deltaTime;

            if (time >= interpolationPeriod)
            {
                time = 0.0f;

                // execute block of code here
                //The Bullet instantiation happens here.
                GameObject Temporary_Bullet_Handler;
                Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position + new Vector3(0.0f, 0.0f, 0.0f), Bullet_Emitter.transform.rotation) as GameObject;

                Temporary_Bullet_Handler.transform.Rotate(Vector3.forward * 100);

                Temporary_Bullet_Handler.transform.Rotate(Vector3.right * 90);
                Temporary_Bullet_Handler.transform.Rotate(Vector3.up * -10);

                Rigidbody Temporary_RigidBody;
                Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

                Temporary_RigidBody.AddForce(transform.up * Bullet_Up_Force);

                Destroy(Temporary_Bullet_Handler, 5.0f);
            }
        }
    }
}