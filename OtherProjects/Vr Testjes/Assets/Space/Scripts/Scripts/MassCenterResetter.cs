using UnityEngine;
using System.Collections;

public class MassCenterResetter : MonoBehaviour {
	public Rigidbody rb;
	public float x;
	public float y;
	public float z;
	// Update is called once per frame
	void LateUpdate () {
		
		rb.centerOfMass = new Vector3 (x, y, z);
	}
}
