using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {
	public float rot;
	public float rotSpeed;
	private Rigidbody rb;

	void Start ()
	{
		rotSpeed = Random.Range(1,30);
		rb = GetComponent <Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * rot;
	}
	void Update(){
		transform.Rotate(this.transform.up *rotSpeed* Time.deltaTime, Space.World);
	}
}