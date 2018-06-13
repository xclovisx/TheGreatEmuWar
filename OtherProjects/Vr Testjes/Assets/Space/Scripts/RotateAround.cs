using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
	public float rotSpeed;
	public bool thiIsPlanet; 
	public Transform Sun;
	public float rotAroundSunSpd;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(Vector3.up *rotSpeed* Time.deltaTime, Space.World);
		if(thiIsPlanet){
			transform.RotateAround(Sun.transform.position, Vector3.up, rotAroundSunSpd * Time.deltaTime);
		}
	}
}
