using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipGateOpener : MonoBehaviour {
	public GameObject[] shipCP;
	public GameObject closest;
	public float distance = 30;
	private float dist;
	private GameObject Player;
	private playerControl plControlScrpt;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		plControlScrpt = Player.GetComponent<playerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab) && plControlScrpt.usingTablet) {
			FinClosestShipCP ();
			if (closest != null) {
				closest.GetComponent<Button> ().onClick.Invoke ();
			}
		}	
	}
	GameObject FinClosestShipCP(){
	shipCP = GameObject.FindGameObjectsWithTag("openPanel"); 
	closest = null;
	Vector3 position = transform.position;
	foreach (GameObject go in shipCP) {
		dist = Vector3.Distance(go.transform.position, position);
		if (dist < distance) {
			closest = go;
		}
	}
	return closest;
	}
}
