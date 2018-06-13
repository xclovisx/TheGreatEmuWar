using UnityEngine;
using System.Collections;

public class OffsetAnim : MonoBehaviour {
	public Material mat;
	public float scrollSpeed = 0.5f;
	public float offset = 0f;//scrolling ammount;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(offset>=1){
			offset = 0;
		}
		offset = Time.time * scrollSpeed;
		mat.SetTextureOffset("_BumpMap", new Vector2(0, offset));
	}
}
