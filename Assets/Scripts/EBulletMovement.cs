using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBulletMovement : MonoBehaviour {

    public Rigidbody m_Rigidbody;
    public float m_Speed;

    void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 50.0f;
        m_Rigidbody.velocity = transform.up * m_Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet"|| other.tag == "EBullet" || other.tag == "Lasernt" || other.tag == "Shield")
        {
            Destroy(gameObject);
        }
        else { Destroy(gameObject, 5); }
    } 
}
