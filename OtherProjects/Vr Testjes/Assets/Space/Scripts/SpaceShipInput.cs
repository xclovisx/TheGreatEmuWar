using UnityEngine;
using System.Collections;

public class SpaceShipInput : MonoBehaviour {
	SpaceShipControl spaceContrlScript;
	// Use this for initialization
	void Start () {
		spaceContrlScript = GetComponent<SpaceShipControl>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float roll = Input.GetAxis ("Mouse X");
		float pitch = Input.GetAxis("Mouse Y");
		bool airBrakes = Input.GetButton("Fire1");
		float throttle = Input.GetAxis("Throttle");

		spaceContrlScript.Move(roll, pitch, 0 , throttle, airBrakes);
	}
}
