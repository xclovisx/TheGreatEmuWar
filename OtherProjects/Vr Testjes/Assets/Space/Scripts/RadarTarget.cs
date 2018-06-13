using UnityEngine;
using System.Collections;

public class RadarTarget : MonoBehaviour {
    private Radar radarScript;
    private GameObject radarObject;
	// Use this for initialization
	void Start () {
        radarObject = GameObject.Find("radar");
        radarScript = radarObject.GetComponent<Radar>();
        radarScript.radarTargets.Add(gameObject.GetComponent<RadarTarget>());
    }

}
