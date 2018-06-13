using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float timeToDestroy;
	public GameObject goToDestroy;
	// Use this for initialization
	void OnEnable () {
		Destroy (goToDestroy, timeToDestroy);
	}
}
