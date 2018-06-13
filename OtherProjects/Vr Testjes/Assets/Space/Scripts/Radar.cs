using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Radar : MonoBehaviour {
	public List<RadarTarget> radarTargets;
	public GameObject indicatorPref;
	private GameObject Player;

	List<GameObject>indicatorPool = new List<GameObject>();
	int indicatorPoolMax = 0;
	// Update is called once per frame
	void Start(){
        List<RadarTarget> radarTargets = new List<RadarTarget>();
        Player = GameObject.FindGameObjectWithTag ("Player");
    }

	void LateUpdate () {
		DrawIndicator ();
	}
	void DrawIndicator(){
		resetPool ();		
		foreach (RadarTarget obj in radarTargets) {
			float dist = Vector3.Distance(obj.transform.position, Player.transform.position);
			GameObject	spawnedIndicator = getIndicator ();
			spawnedIndicator.GetComponent<Text>().text = Mathf.Round(dist/10) +"km";
			Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
			//check if indicator is offscreen
			if(screenPos.z<0){
				spawnedIndicator.SetActive(false);
			}
			else{
				spawnedIndicator.SetActive(true);
			}
			//setting flat
			screenPos = new Vector3 (screenPos.x, screenPos.y, 0);
			spawnedIndicator.transform.position = screenPos;
		}
		cleanPool ();
	}
	void resetPool(){
		indicatorPoolMax = 0;
	}
	GameObject getIndicator(){
		GameObject output;
		if (indicatorPoolMax < indicatorPool.Count) {
			output = indicatorPool [indicatorPoolMax];
		} else {
			output = Instantiate (indicatorPref) as GameObject;
			output.transform.SetParent(this.transform);
			indicatorPool.Add (output);
		}
		indicatorPoolMax++;
		return output;
	}
	void cleanPool(){
		while (indicatorPool.Count > indicatorPoolMax) {
			GameObject obj = indicatorPool [indicatorPool.Count - 1];
			indicatorPool.Remove (obj);
			Destroy (obj.gameObject);
		}
	}
}