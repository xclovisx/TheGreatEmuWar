using UnityEngine;
using System.Collections;

public class UVScroller : MonoBehaviour {

	public float scrollSpeed = 0.5F;
	public Material mat;
	private float offset = 0;
	void Start() {
		offset = 0;
	}
	void Update() {
		offset = Time.time * scrollSpeed;
		mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}