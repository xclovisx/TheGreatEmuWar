using UnityEngine;
using System.Collections;

public class PlayOneAudio : MonoBehaviour {
	public AudioSource aSource;
	public AudioClip openClip;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OpenSound(){
		if(aSource!=null){
			aSource.PlayOneShot (openClip);
		}
	}
}
