using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  script gebruikt bij  {Laser spawner} script  bij laser.
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
