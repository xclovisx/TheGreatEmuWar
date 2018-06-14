using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor")
        {
            if (counter.hpCounter > 0) { counter.hpCounter -= 4;  }

        }
       if (other.tag == "EBullet")
        {
            if (counter.hpCounter > 0) { counter.hpCounter -= 1; }

        }


    }
}
