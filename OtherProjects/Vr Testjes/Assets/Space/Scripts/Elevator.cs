using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	public Transform thisTransf;
	public bool inElevator;
	public bool goingDown;
	public bool goingUp;
	public float maxHeight;
	public float minHeight;
	public float liftSpeed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (inElevator) {
			if (goingDown && thisTransf.localPosition.y > minHeight) {
				thisTransf.Translate (Vector3.up * -liftSpeed * Time.deltaTime);
			}
			if (goingUp && thisTransf.localPosition.y < maxHeight) {
				thisTransf.Translate (Vector3.up * +liftSpeed * Time.deltaTime);
			}else {
				return;
				}
		}	
	}
	void OnTriggerEnter (Collider coll){
		if (coll.gameObject.tag == "Player") {
			coll.transform.parent = this.transform;
			inElevator = true;
		}
	}
	void OnTriggerExit (Collider coll){
		if (coll.gameObject.tag == "Player") {
			coll.transform.parent = null;
			inElevator = false;
			goingDown = false;
			goingUp = false;
		}
	}
	public void GoingDown(){
		goingDown = true;
		goingUp = false;
	}
	public void GoingUp(){
		goingDown = false;
		goingUp = true;
	}
}
