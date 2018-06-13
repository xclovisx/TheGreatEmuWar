using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour
{
	private Animator _anim;
	public float speed = 90f;
	public float turnSpeed = 5f;
	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;
	private float powerInput;
	private float turnInput;
	private float scrollSpeed = 2f;
	private Rigidbody carRigidbody;
	public bool canControl;
	public Transform LHandPos;
	public Transform RHandPos;

	void Awake (){
		carRigidbody = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
	}

	void Update (){
		if(canControl){
			powerInput = Input.GetAxis ("Vertical");
			turnInput = Input.GetAxis ("Horizontal");
			carRigidbody.AddRelativeForce (Vector3.forward * speed * powerInput);
			carRigidbody.AddRelativeTorque (0f, turnInput * turnSpeed, 0f);
		}
	}

	void FixedUpdate ()	{
		if(canControl){
			Ray ray = new Ray (transform.position, -transform.up);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, hoverHeight)) {
				float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
				Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
				carRigidbody.AddForce (appliedHoverForce, ForceMode.Acceleration);
			}
			carRigidbody.AddRelativeForce (Vector3.forward *powerInput * speed);
			carRigidbody.AddRelativeTorque (0f, turnInput * turnSpeed, 0f);

			Vector3 predictedUp = Quaternion.AngleAxis (
				                            carRigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * 0.3f / speed, carRigidbody.angularVelocity) * transform.up;
			Vector3 torqueVector = Vector3.Cross (predictedUp, Vector3.up);
			carRigidbody.AddTorque (torqueVector * speed * speed);


			if (Input.GetKey (KeyCode.Space)) {
				carRigidbody.drag = Mathf.Lerp (carRigidbody.drag, 5, Time.deltaTime / 2);
			} else {
				carRigidbody.drag = Mathf.Lerp (carRigidbody.drag, 0.2f, Time.deltaTime);
			}
			_anim.SetFloat("turn", turnInput);

			hoverHeight += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
			if(hoverHeight>3.5f){
				hoverHeight=3.5f;
			}
			if(hoverHeight<1){
				hoverHeight=1f;
			}
		}

	}

}