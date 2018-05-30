using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

    public Rigidbody m_Rigidbody;
    public float m_Speed;

    void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 10.0f;
        m_Rigidbody.velocity = transform.up * m_Speed;
    }
	
	void Update () {
    }
}
