using UnityEngine;
using System.Collections;

public class carDocking : MonoBehaviour {
		public bool CarDocked;
		public GameObject curSpaceShip;
		private SpaceShipControl sShipCntrlScrt;
		public AudioClip carDocked;
		public AudioClip carLeft;
		private AudioSource auSource;

	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider col){
		if(col.gameObject.name == "carDoc"){
				//this.GetComponent<Rigidbody> ().isKinematic = true;
				this.transform.parent = col.gameObject.transform;	
				CarDocked = true;
				//m_Car.Move (0, 0, 0, 1);
				curSpaceShip = col.gameObject.transform.root.gameObject;
				sShipCntrlScrt = curSpaceShip.GetComponent<SpaceShipControl> ();
				sShipCntrlScrt.dockedCar = this.gameObject;
				auSource = this.gameObject.transform.parent.GetComponent<AudioSource>();
				auSource.spatialBlend = 0.7f;
				auSource.clip = carDocked;
				auSource.loop = false;
				if (!auSource.isPlaying){
					auSource.Play();
				}
			}
		}
		void OnTriggerExit(Collider col){
			if (col.gameObject.name == "carDoc") {				
				auSource.clip = carLeft;
				auSource.loop = false;
				if (!auSource.isPlaying){
					auSource.Play();
				}
				this.transform.parent = null;
				CarDocked = false;
				curSpaceShip = null;
				sShipCntrlScrt.dockedCar = null;
			}
		}
	}