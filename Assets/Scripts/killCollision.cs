using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
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
