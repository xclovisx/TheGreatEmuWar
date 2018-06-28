using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public Transform target;
    public float speed;
    private int stop = 0;

    void Update()
    {
        if (stop == 0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield")
        {
            stop += 1;
        }
    }
}