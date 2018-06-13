using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
	private Light lightFlcr;
	private AudioSource aSource;
	// Use this for initialization
	void Start () {
		lightFlcr = this.GetComponent<Light> ();
		aSource = this.GetComponent<AudioSource> ();
	}
	void OnEnabled(){
		aSource.Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float rndNum = Random.value;
		if (rndNum <= 0.7f) {
			lightFlcr.enabled = true;
		} else {
			lightFlcr.enabled = false;
		}
	}
}
