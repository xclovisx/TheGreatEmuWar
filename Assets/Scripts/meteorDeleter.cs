using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  script gebruikt bij  {Assets>Models>Astroide>Astroïde_Model 2} script  bij Astroïde_Model 2 = een prefab

public class meteorDeleter : MonoBehaviour
{
    private void Start()
    {
        
    }

    void spawn()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield" || other.tag == "EBullet")
        {   
                

        }/*
       if (other.tag == "EBullet")
        { if (counter.hpCounter > 0) { counter.hpCounter -= 1; } }*/
     
        else
        {
            Destroy(gameObject);
            // Destroy(gameObject);
            // counter ct = GetComponent<counter>();
            // ct.points += 100;
        }


    }

}
