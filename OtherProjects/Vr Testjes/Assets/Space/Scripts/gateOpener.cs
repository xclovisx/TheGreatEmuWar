using UnityEngine;
using System.Collections;

public class gateOpener : MonoBehaviour {
	public GameObject Gate;
	private Animator _anim;
	// Use this for initialization
	void Start () {
		_anim = Gate.GetComponent<Animator>();

	}
	void OpenGate(){
		_anim.SetTrigger("OpenGate");
	}
	void CloseGate(){
		_anim.SetTrigger("CloseGate");
	}

}
