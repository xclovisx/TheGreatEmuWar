using UnityEngine;
using System.Collections;
//  script gebruikt bij  {--} script bij Alien en Meteor


public class Explosion : MonoBehaviour
{

    public GameObject Meteor;

    public GameObject ExplosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield" || other.tag == "EBullet" || other.tag == "lasernt")
        {
            //Er gebeurt hier niks want Als hij deze raakt gebeurt er niks...
        }

        else
        {
            //Debug.Log("Pressed");
            //The Explosion instantiation happens here.
            GameObject Temporary_Explosion_Handler;
            Temporary_Explosion_Handler = Instantiate(ExplosionEffect, Meteor.transform.position + new Vector3(0.0f, 0.0f, 0.0f), Meteor.transform.rotation) as GameObject;

            Temporary_Explosion_Handler.transform.Rotate(Vector3.forward * 90);
            Temporary_Explosion_Handler.transform.Rotate(Vector3.right * -90);
            Temporary_Explosion_Handler.transform.Rotate(Vector3.up * -90);

            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Explosion_Handler.GetComponent<Rigidbody>();

            Destroy(Temporary_Explosion_Handler, 5.0f);


        }
    }
}