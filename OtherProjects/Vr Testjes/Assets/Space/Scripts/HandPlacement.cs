using UnityEngine;
using System.Collections;

public class HandPlacement : MonoBehaviour {
	public Animator _anim;
	public Transform LeftHandPos;
	public Transform RightHandPos;
	public bool handIk;
	// Use this for initialization
	void Start () {
		_anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnAnimatorIK(int layerIndex){
		//Left hand
		if(handIk){
			_anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
			_anim.SetIKPosition (AvatarIKGoal.LeftHand, LeftHandPos.position);
			_anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
			_anim.SetIKRotation (AvatarIKGoal.LeftHand, LeftHandPos.rotation);
			//Right hand
			_anim.SetIKPositionWeight (AvatarIKGoal.RightHand, 1);
			_anim.SetIKPosition (AvatarIKGoal.RightHand, RightHandPos.position);
			_anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
			_anim.SetIKRotation (AvatarIKGoal.RightHand, RightHandPos.rotation);
		}
	}
}
