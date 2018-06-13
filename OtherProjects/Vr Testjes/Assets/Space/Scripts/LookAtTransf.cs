using UnityEngine;
using System.Collections;

public class LookAtTransf : MonoBehaviour {
	public Transform Player;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		this.transform.LookAt(Player);
	}
}
