using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {
	public bool hasIndicator;
	public GameObject indicatorPref;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public GameObject getIndicator(){
		if(!hasIndicator){
		GameObject output;
		output = (GameObject)Instantiate (indicatorPref);
			hasIndicator= true;
		//output.transform.parent = transform;
		return output;
		}
		else{
			return null;
		}
	}
}
