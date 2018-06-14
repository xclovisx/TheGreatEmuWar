using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor")
        {
            counter.killCounter += 10; 

        }
        else if (other.tag == "EBullet")
        {
            counter.killCounter += 2; 

        }
     


    }
}
