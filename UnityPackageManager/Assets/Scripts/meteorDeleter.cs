using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorDeleter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
