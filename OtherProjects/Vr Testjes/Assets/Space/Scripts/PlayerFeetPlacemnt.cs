using UnityEngine;
using System.Collections;

public class PlayerFeetPlacemnt : MonoBehaviour {
	private Animator _anim;
	private GameObject Player;
	private playerControl plControlScrpt;

	Vector3 lFpos;
	Vector3 rFpos;

	Quaternion lFrot;
	Quaternion rFrot;

	float lFweight;
	float rFweight;

	Transform rightFoot;
	Transform leftFoot;

	public float offsetY;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		plControlScrpt = Player.GetComponent<playerControl>();
		_anim = this.GetComponent<Animator> ();
		leftFoot = _anim.GetBoneTransform (HumanBodyBones.LeftFoot);
		rightFoot = _anim.GetBoneTransform (HumanBodyBones.RightFoot);

		lFrot = leftFoot.rotation;
		rFrot = rightFoot.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit leftHit;
		RaycastHit rightHit;

		Vector3 lPos = leftFoot.TransformPoint (Vector3.zero);
		Vector3 rPos = rightFoot.TransformPoint (Vector3.zero);

		if (Physics.Raycast (lPos, -Vector3.up, out leftHit, 1)) {
			lFpos = leftHit.point;
			lFrot = Quaternion.FromToRotation(transform.up, leftHit.normal)*transform.rotation;
		}
		if (Physics.Raycast (rPos, -Vector3.up, out rightHit, 1)) {
			rFpos = rightHit.point;
			rFrot = Quaternion.FromToRotation(transform.up, rightHit.normal)*transform.rotation;
		}

	}
	void OnAnimatorIK(){
		if(plControlScrpt.canUseIK){
		lFweight = _anim.GetFloat ("LeftFoot");
		rFweight = _anim.GetFloat ("RightFoot");
//position
		float distToLFoot = Vector3.Distance (Player.transform.position, lFpos);
		if(distToLFoot<1f){
			_anim.SetIKPositionWeight (AvatarIKGoal.LeftFoot, lFweight);
			_anim.SetIKPositionWeight (AvatarIKGoal.RightFoot, rFweight);
		}
		_anim.SetIKPosition (AvatarIKGoal.LeftFoot, lFpos + new Vector3(0, offsetY, 0));
		_anim.SetIKPosition (AvatarIKGoal.RightFoot, rFpos + new Vector3(0, offsetY, 0));
//rotation
		float distToRFoot = Vector3.Distance (Player.transform.position, rFpos);
		if(distToRFoot<1f){
			_anim.SetIKRotationWeight (AvatarIKGoal.LeftFoot, lFweight);
			_anim.SetIKRotationWeight (AvatarIKGoal.RightFoot, rFweight);
			}
		_anim.SetIKRotation (AvatarIKGoal.LeftFoot, lFrot);
		_anim.SetIKRotation (AvatarIKGoal.RightFoot, rFrot);
		}
	}
}