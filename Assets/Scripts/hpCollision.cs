using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  script gebruikt bij  {Environment/Room/platform_spaceship4/spaceship_2} script  bij spaceship_2 
public class hpCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor")
        {
            if (counter.hpCounter > 0) { counter.hpCounter -= 4;  }

        }
       if (other.tag == "EBullet")
        {
            if (counter.hpCounter > 0) { counter.hpCounter -= 1;  }

        }


    }
}
