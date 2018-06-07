using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class meteorDeleter : MonoBehaviour
{
    public int count = 0;
    private int points;
    private void Start()
    {
        count = 0;
    }

    void spawn()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield" || other.tag == "EBullet")
        {

        }
        else
        {
            Destroy(gameObject);
            counter ct = GetComponent<counter>();
            ct.points += 100;
        }


    }

}
