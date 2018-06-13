using UnityEngine;
using System.Collections;

public class lookAt : MonoBehaviour {
	public Transform Player;
	public float x;
	public float y;
	public float z;
	public bool LookAtPlayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(LookAtPlayer){
			this.transform.LookAt(Player);
		}
		this.transform.position = Player.position + new Vector3 (x, y, z);
	}
}
