using UnityEngine;
using System.Collections;

public class CamLook : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 mousePos = Input.mousePosition;
		//mousePos.z = Camera.main.nearClipPlane;
		mousePos.z = 100f;
		Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
		transform.LookAt(lookPos);
	}
}
