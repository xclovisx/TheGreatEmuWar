using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class counter : MonoBehaviour {

    public int points;
    public Text kill;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        kill.text = points.ToString();
    }
}
