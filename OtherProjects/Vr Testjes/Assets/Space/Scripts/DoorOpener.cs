using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {
	private GameObject Player;
	public Animator _anim;
	private bool doorOpened;
	private bool doorClosed;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Player != null) {
			float dist = Vector3.Distance (Player.transform.position, transform.position); 
			if (dist < 3) {
				doorOpened = true;
				doorClosed = false;
			} else {
				doorClosed = true;
				doorOpened = false;
			}
		} else {
			return;
		}
	}
	void FixedUpdate(){
		_anim.SetBool ("Open", doorOpened);
		_anim.SetBool ("Close", doorClosed);
	}
}
