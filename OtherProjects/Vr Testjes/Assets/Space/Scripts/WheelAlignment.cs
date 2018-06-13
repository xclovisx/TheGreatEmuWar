using UnityEngine;
using System.Collections;

public class WheelAlignment : MonoBehaviour {
	public WheelCollider m_WheelCollider;
	public GameObject m_WheelMesh;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion quat;
		Vector3 position;
		m_WheelCollider.GetWorldPose(out position, out quat);
		m_WheelMesh.transform.position = position;
		m_WheelMesh.transform.rotation = quat;
	}
}
