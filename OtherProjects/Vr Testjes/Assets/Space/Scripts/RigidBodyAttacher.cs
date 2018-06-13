//we need to disable all the rigidbodies except main rigidbody, so they will not interfere ship movement
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyAttacher : MonoBehaviour {
    public GameObject[] rbHolders;
    private SpaceShipControl shipContrl;
	// Use this for initialization
	void Start () {
        shipContrl = GetComponent<SpaceShipControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (shipContrl.landed) {
            foreach (GameObject rbHold in rbHolders) {
                rbHold.SetActive(true);
            }
        }
        if (!shipContrl.landed)
        {
            foreach (GameObject rbHold in rbHolders)
            {
                rbHold.SetActive(false);
            }
        }
    }
}
