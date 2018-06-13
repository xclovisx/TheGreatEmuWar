using UnityEngine;
using System.Collections;

public class EmisionFlickering : MonoBehaviour {
	public Material mat;
	public Color colr;
	// Use this for initialization
	void Start () {
	
	}
	
	void LateUpdate () {
		float emission = Mathf.PingPong (Time.deltaTime*10, 10f);
		Color finalColor = colr * Mathf.LinearToGammaSpace (emission);
		mat.SetColor ("_EmissionColor", finalColor);
	}
}