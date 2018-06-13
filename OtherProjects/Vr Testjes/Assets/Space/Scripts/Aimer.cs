using UnityEngine;
using System.Collections;

public class Aimer : MonoBehaviour {
	private GameObject Player;
	private playerControl plyrScrpt;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		plyrScrpt = Player.GetComponent<playerControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(plyrScrpt.lookAtPos.transform);
	}
}
