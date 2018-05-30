using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorDeleter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield" || other.tag == "EBullet")
        {
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
