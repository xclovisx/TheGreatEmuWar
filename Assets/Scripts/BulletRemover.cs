using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemover : MonoBehaviour {

	void Update () {
        Destroy(gameObject, 10);
    }
}
