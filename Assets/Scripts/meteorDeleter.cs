using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

//  script gebruikt bij  {Assets>Models>Astroide>Astroïde_Model 2} script  bij Astroïde_Model 2 = een prefab

public class meteorDeleter : MonoBehaviour
{ 
    //////////////////////////////////
    public AudioClip explosionSound;

    private AudioSource source;
    /////////////////////////////////

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield" || other.tag == "EBullet" || other.tag == "lasernt")
        {   
                //Er gebeurt hier niks want Als hij deze raakt gebeurt er niks...
        }/*
       if (other.tag == "EBullet")
        { if (counter.hpCounter > 0) { counter.hpCounter -= 1; } }*/
     
        else
        {
            source.PlayOneShot(explosionSound, 1);
            Debug.Log("test");

            Destroy(gameObject);
            // Destroy(gameObject);
            // counter ct = GetComponent<counter>();
            // ct.points += 100;
        }


    }

}
