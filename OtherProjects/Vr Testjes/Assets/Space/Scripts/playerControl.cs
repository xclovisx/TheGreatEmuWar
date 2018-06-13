using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerControl : MonoBehaviour {
	private Component[] rigidBodies;//array of character's rigidbodies
	public Collider [] ragDallColliders;//array of character's collider
	private Animator _animator;
	private AnimatorStateInfo currentBaseState;
	private CharacterController _collMain;
	private LayerMask characterMask = 5;
	private Camera MainCam;

	[Header("-SOUND-")]
	private AudioSource m_AudioSource;
	public AudioClip[] m_FootstepSounds;
	public AudioClip[] m_MetalFootstepSounds;
	public AudioClip[] m_BodyHit;
	public AudioClip m_jumpSound;

	[Header("-IK-")]
	public float lookWeight;
	public float jumpSpeed;
	public float RHandWeight;
	public Transform hintRight;
	public Transform hintLeft;
	public Transform rifleLeftIkPos;
	public Transform pistolIkPos;
	public Transform  myHand;
	public Transform  pistolHandHolder;
	public Transform rifleHolster;
	public Transform pistolHolster;
	public float lookIKweight;
	public float bodyWeight;
	public float headWeight;
	public float eyesWeight;
	public float clampWeight;
	private float chairHandWeight;
	public Transform lookAtPos;
	public Transform endPos;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 charCollXYZ;

	[Header("-INPUT-")]
	private float _mouseY;
	private float _mouseX;
	private float _speed;
	private float _strafe;
	private float _distToGround;
	private float shootTime = 0;

	[Header("-PLAYER ATTR-")]
	public float gravity=9.8f;
	public float health;
	private bool _crouch;
	private bool _jumpButton;
	private bool _run;
	private bool _canLook;
	private bool _inChair;
	private bool _canPilot;
	private bool _canShoot;

	public bool underFire;
	public bool shooting;
	public bool driving;
	public bool usingPistol;
	public bool usingRifle;
	public bool usingTablet;
	public bool atWall;
	public bool canRotate;
	public bool canUseIK;
	public bool alive;
	public bool inSpace;
	private bool canSwitchweapon = true;

	[Header("-WEHICLES-")]
	public GameObject curHover;
	public GameObject curChair;
	public GameObject curShip;
	public GameObject curCar;
	public GameObject curMech;

	public GameObject _carRightDoor;
	public GameObject _carLeftDoor;

	public GameObject cursorUnarmed;
	public GameObject cursorPistol;
	public GameObject cursorRiffle;
	public GameObject tablet;

	//ANIMATION STATES
	static int jumpState = Animator.StringToHash("Base Layer.JumpUpHigh");
	static int jumpShortState = Animator.StringToHash("Base Layer.jumpShort");

	[Header("-WEAPONS-")]
	
	/// assign all the values in the inspector
	
	public WeaponInfo currentWeapon;
	public List<WeaponInfo> WeaponList = new List<WeaponInfo>();
	[System.Serializable]
	public class WeaponInfo{
		public string weaponName = "veapon name";
		public float fireRate = 0.1f;
		public Transform weaponTransform;
		public Transform weaponMuzzlePoint;
		public float weaponKickBack;
		public GameObject bulletPrefab;
		public int totalAmmo;
		public int magazine;
		public AudioSource gunAS;
		public AudioClip gunShootClip;
	}
	// Use this for initialization
	void Start () {
		m_AudioSource = GetComponent<AudioSource>();
		MainCam = Camera.main;
		_animator = gameObject.GetComponent<Animator> ();
		_collMain = gameObject.GetComponent<CharacterController>();
		charCollXYZ = _collMain.center;
		rigidBodies = GetComponentsInChildren<Rigidbody>();//our character is a ragdoll, lets collect its rigidbodies
		_canShoot = false;
		currentWeapon = WeaponList[0];	
		if(inSpace){
		MainCam.clearFlags= CameraClearFlags.SolidColor;
		}
		else{
			MainCam.clearFlags= CameraClearFlags.Skybox;
		}
		foreach (Rigidbody rb in rigidBodies){//lets make all reigidbodies of the character kinematic
			rb.isKinematic = true;
		}
		foreach (Collider col in ragDallColliders){
			col.enabled = false;//lets make all the colliders of the character disabled
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(alive){
		_crouch = Input.GetKey(KeyCode.C) ? true : false;//check if run button was pressed
		_mouseX = Input.GetAxis ("Mouse X");
		_speed = Input.GetAxis("Vertical");//reading vertical axis input
		_strafe = Input.GetAxis("Horizontal");//reading horizontal axis input
		_jumpButton = Input.GetButton("Jump") ? true : false;//check if jump button was pressed
		_run = Input.GetKey(KeyCode.LeftShift) ? true : false;//check if run button was pressed
//change character comtrollr's parameters when player is slideing
		if(_animator.GetCurrentAnimatorStateInfo(0).IsName("RunSlide")){//hash it <--
			_collMain.height = _animator.GetFloat("CollHeight");//this value can be found in sliding animation curves
			charCollXYZ = new Vector3(charCollXYZ.x, _animator.GetFloat("CollPos"), charCollXYZ.z);//this value can be found in sliding animation curves
			_collMain.center = charCollXYZ;
			}else{
				_collMain.height = 3.12f;
				charCollXYZ.y = 1.6f;
				_collMain.center = charCollXYZ;
			}
		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		lookAtPos.position = ray.GetPoint (15);
//PROCESS SHOOTING
		if (Input.GetMouseButton (0) && _canShoot) {
			if (shootTime <= Time.time) {
				shootTime = Time.time + currentWeapon.fireRate;
				ShootRay ();
				shooting = true;
				if (usingRifle) {
					_animator.SetTrigger ("RiffleShot");
					currentWeapon.gunAS.PlayOneShot (currentWeapon.gunShootClip);
				}
				if (usingPistol) {
					_animator.SetTrigger ("pistolShot");
					currentWeapon.gunAS.PlayOneShot (currentWeapon.gunShootClip);
				}
				currentWeapon.magazine -= 1;
				if (currentWeapon.magazine < 0) {
					currentWeapon.magazine = 0;
				}
				if (currentWeapon.magazine == 0 && currentWeapon.totalAmmo > 0) {
						
					_animator.SetTrigger ("ReloadWeapon");	
				}
			}
			else {
				shooting = false;
			}
		} 
		else{
			shooting = false;
		}
			//PROCESSING ROTATION
			Vector3 aimPoint =  Camera.main.transform.forward*10f;
			if(!atWall && canRotate){
				Quaternion targetRotation = Quaternion.LookRotation(aimPoint);				
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 250 * Time.deltaTime);
                this.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
//Applying Gravity

		if(isGrounded()){
			if (Input.GetButton ("Jump")) {				
				_animator.SetTrigger ("jump");
				if(_animator.GetBool("jump") == true){
					moveDirection.y = jumpSpeed;
				}
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		if(_collMain.enabled){
		_collMain.Move (moveDirection * Time.deltaTime);
		if(moveDirection.y < -15f){
			moveDirection.y = -15f;
		}
		}

		if((currentBaseState.fullPathHash == jumpState)||(currentBaseState.fullPathHash == jumpShortState)){
			_animator.ResetTrigger("jump");
		}
		//WEAPON PROCESSING
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			if(!usingRifle){
				StartCoroutine(SwapRifle());
			}
			if(usingRifle){
				StartCoroutine(HideRifle());
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			if(!usingPistol){
				StartCoroutine(SwapPistol());
			}
			if(usingPistol){
				StartCoroutine(HidePistol());
			}
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			if (!usingTablet) {
				StartCoroutine(SwapTablet());
			}
			if (usingTablet) {
				StartCoroutine(HideTablet());
			}
		}
//SHOOTING RAY FORWARD
		Vector3 ahead = transform.forward;
		Vector3 rayStart = new Vector3(this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
		Ray	rayFrwrd = new Ray(rayStart, ahead);
		RaycastHit hit;
		Debug.DrawRay(rayStart, transform.forward, Color.green);
		if(Physics.Raycast(rayFrwrd, out hit, 2f)){
			if(hit.transform.gameObject.name == ("chair")){
				curChair = hit.transform.gameObject;
			}
		}
		if (curChair != null) {
			float dist = Vector3.Distance(curChair.transform.position, this.transform.position);
			if(dist>2){
				curChair = null;
				curShip = null;
			}
		}
		if(curMech!=null){
			float distToMech = Vector3.Distance(curMech.transform.position, this.transform.position);
			if(distToMech>6){
					curMech = null;
			}
		}
		if (Physics.Raycast (rayFrwrd, out hit, 1f)) {
			if (hit.transform.gameObject.name == ("spaceShipRight")) {
				curShip = hit.transform.parent.gameObject;
				_canPilot = true;
			}
		}
		if (curShip != null && curChair == null) {
			float distToSship = Vector3.Distance (curShip.transform.position, this.transform.position);
			if (distToSship > 4) {
				curShip = null;
				_canPilot = false;
			}
		}
		if(Physics.Raycast(rayFrwrd, out hit, 1f)){
			if(hit.transform.gameObject.name == ("doorR")){
				curCar = hit.transform.parent.gameObject;
				_carRightDoor = hit.transform.gameObject;
			}
			if(hit.transform.gameObject.name == ("doorL")){
				curCar = hit.transform.parent.gameObject;
				_carLeftDoor = hit.transform.gameObject;
			}
			if(hit.transform.gameObject.name == ("hover")){
				curHover = hit.transform.gameObject;
			}
			if(hit.transform.gameObject.name == ("mechDetect")){
				curMech = hit.transform.gameObject.transform.parent.gameObject;
			}
		}
		if (curHover != null) {
			float distToHover = Vector3.Distance(curHover.transform.position, this.transform.position);
			if(distToHover>3){
				curHover = null;
			}
		}

		if (curCar != null) {
			if (_carLeftDoor != null) {
				float distToLDoor = Vector3.Distance (_carLeftDoor.transform.position, this.transform.position);
				if (distToLDoor > 4) {
					curCar = null;
					_carLeftDoor = null;
				}
			}
			if (_carRightDoor != null) {
				float distToRDoor = Vector3.Distance (_carRightDoor.transform.position, this.transform.position);
				if (distToRDoor > 4) {
					curCar = null;
					_carRightDoor = null;
				}
			}
		}
			if (curCar &&!driving && Input.GetKeyDown(KeyCode.E) && _collMain.enabled == true && curChair == null) {
			StartCoroutine(GetInCar ());
		}
			if (driving && curChair==null && Input.GetKeyDown (KeyCode.H)) {
			StartCoroutine (GetOutCar ());
		}
		_animator.SetFloat("mouseX",_mouseX, 0.3f, Time.deltaTime);
		_animator.SetFloat("Speed", _speed);
		_animator.SetFloat("Strafe", _strafe);
		_animator.SetBool("grounded", isGrounded());
		_animator.SetBool("Crouch", _crouch);
		_animator.SetBool("Jump", _jumpButton);
		_animator.SetBool ("Run", _run);
		_animator.SetFloat ("VertVelocity", _collMain.velocity.y);
		currentBaseState = _animator.GetCurrentAnimatorStateInfo(0);

		if(Input.GetKeyDown(KeyCode.E)){
			if((curChair != null)&&(_collMain.enabled)&&(canUseIK)){								
				StartCoroutine(SitInChair());
			}
			if (_canPilot && canRotate) {
				StartCoroutine (GetInShip ());
				}
			if(curHover && _collMain.enabled){
				StartCoroutine(GetOnHover());
			}
			if (curMech && _collMain.enabled) {
				StartCoroutine(GetOnMech());
			}
		}
		if(Input.GetKeyDown(KeyCode.H)){
			if(_inChair && !_collMain.enabled && curChair!=null){
                    StartCoroutine(GetUpChair());               
			    }
			if(_canPilot && curShip.GetComponent<SpaceShipControl>().landed){
				StartCoroutine (GetOutShip());
			}
			if (curHover != null) {
				if (curHover.GetComponent<Hover> ().canControl) {
					StartCoroutine (GetOffHover ());
				}
			}
			if (curMech != null) {
				if (curMech.GetComponent<MechControll> ().canControll) {
					StartCoroutine (GetOffMech ());
				}
			}
		}
	}
	}
//IK
	void OnAnimatorIK(int layerIndex)
	{
		if(canUseIK){
			_animator.SetLookAtPosition (lookAtPos.position);
			_animator.SetLookAtWeight (lookIKweight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}
		if (usingRifle) {
			_animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, RHandWeight);
			_animator.SetIKPosition (AvatarIKGoal.LeftHand, rifleLeftIkPos.position);
			_animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, RHandWeight);
			_animator.SetIKRotation (AvatarIKGoal.LeftHand, rifleLeftIkPos.rotation);
			_animator.SetIKHintPositionWeight (AvatarIKHint.LeftElbow, 1f);
			_animator.SetIKHintPosition (AvatarIKHint.LeftElbow, hintRight.position);
		}
		if (usingPistol) {			
			_animator.SetIKPositionWeight (AvatarIKGoal.RightHand, RHandWeight);
			_animator.SetIKPosition (AvatarIKGoal.RightHand, pistolIkPos.position);
			_animator.SetIKRotationWeight (AvatarIKGoal.RightHand, RHandWeight);
			_animator.SetIKRotation (AvatarIKGoal.RightHand, pistolIkPos.rotation);
			_animator.SetIKHintPositionWeight (AvatarIKHint.RightElbow, 1f);
			_animator.SetIKHintPosition (AvatarIKHint.RightElbow, hintLeft.position);
		}
		//placing a hand on a joystic
		if(curChair!=null){
			GameObject handPos = curChair.transform.Find("handHold").gameObject;
			_animator.SetIKPositionWeight (AvatarIKGoal.RightHand, chairHandWeight);
			_animator.SetIKPosition (AvatarIKGoal.RightHand, handPos.transform.position);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, chairHandWeight);
            _animator.SetIKRotation (AvatarIKGoal.RightHand, handPos.transform.rotation);
			_animator.SetIKHintPositionWeight (AvatarIKHint.RightElbow, chairHandWeight);
			_animator.SetIKHintPosition (AvatarIKHint.RightElbow, hintLeft.position);
		}

	}
	bool isGrounded()	{
		return Physics.CheckSphere(transform.position, 1f, characterMask | 1<<9);
	}
	public void jump(){
		
	}

//SHOOTING LOGIC
	void ShootRay(){
		float x = Screen.width / 2;
		float y = Screen.height / 2;
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (x, y, 0));
		RaycastHit hit;
		GameObject bullet = Instantiate(currentWeapon.bulletPrefab, currentWeapon.weaponMuzzlePoint.position, currentWeapon.weaponMuzzlePoint.rotation) as GameObject;
		LineRenderer bulletTrail = bullet.GetComponent<LineRenderer> ();
		Vector3 startPos = currentWeapon.weaponMuzzlePoint.TransformPoint (Vector3.zero);
		Vector3 endPos = Vector3.zero;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				float distance = Vector3.Distance(currentWeapon.weaponMuzzlePoint.transform.position, hit.point);
				RaycastHit[] hits = Physics.RaycastAll(startPos, hit.point - startPos, distance);
				currentWeapon.weaponMuzzlePoint.transform.LookAt(hit.point);
			foreach(RaycastHit hit2 in hits){
				if(hit2.transform.GetComponent<Rigidbody>()){
					Vector3 direction = hit2.transform.position - transform.position;
					direction = direction.normalized;
					hit2.transform.GetComponent<Rigidbody>().AddForce(direction * 200);
					}
				if(hit2.collider.transform.root.transform.GetComponent<Enemy>()){
					hit2.collider.transform.root.transform.GetComponent<Enemy>().ApplyDamage(20);
					}
				}
			endPos = hit.point;
			}
			else{//ray, out hit, Mathf.Infinity				
				currentWeapon.weaponMuzzlePoint.transform.LookAt(ray.GetPoint(100));
				endPos = ray.GetPoint(100);
			}
		bulletTrail.SetPosition (0, startPos);
		bulletTrail.SetPosition (1, endPos);
	}
//SWITCHING TO RIFFLE
	IEnumerator SwapRifle(){
		if (canSwitchweapon) {
			canSwitchweapon = false;
			if (usingTablet) {
				_animator.SetBool ("tabletActive", false);
				yield return new WaitForSeconds (0.5f);
				usingTablet = false;
				tablet.SetActive (false);
			}
			if (usingPistol) {
				myHand.GetComponent<Aimer> ().enabled = false;
				RHandWeight = 0;
				_canShoot = false;
				_animator.SetBool ("pistolActive", false);
				yield return new WaitForSeconds (0.7f);
				usingPistol = false;
				WeaponList [1].weaponTransform.position = pistolHolster.transform.position;
				WeaponList [1].weaponTransform.rotation = pistolHolster.transform.rotation;
				WeaponList [1].weaponTransform.parent = pistolHolster;
			}
			_animator.SetBool ("rifleActive", true);
			currentWeapon = WeaponList [0];
			yield return new WaitForSeconds (0.7f);	
			usingRifle = true;
			usingPistol = false;
			cursorRiffle.SetActive (true);
			cursorPistol.SetActive (false);
			cursorUnarmed.SetActive (false);
			WeaponList [0].weaponTransform.parent = myHand.transform;
			WeaponList [0].weaponTransform.position = myHand.transform.position;
			WeaponList [0].weaponTransform.localEulerAngles = new Vector3 (0, 100, 0);
			yield return new WaitForSeconds (1.5f);
			myHand.GetComponent<Aimer> ().enabled = true;
			_canShoot = true;
			RHandWeight = 1;
			canSwitchweapon = true;
		}
	}
	IEnumerator HideRifle(){
		if (canSwitchweapon) {
			canSwitchweapon = false;
			myHand.GetComponent<Aimer> ().enabled = false;
			RHandWeight = 0;
			_canShoot = false;
			_animator.SetBool ("rifleActive", false);
			currentWeapon = WeaponList [0];
			yield return new WaitForSeconds (1.3f);	
			usingRifle = false;
			cursorRiffle.SetActive (false);
			cursorPistol.SetActive (false);
			cursorUnarmed.SetActive (true);
			WeaponList [0].weaponTransform.position = rifleHolster.transform.position;
			WeaponList [0].weaponTransform.rotation = rifleHolster.transform.rotation;
			WeaponList [0].weaponTransform.parent = rifleHolster.transform;
			canSwitchweapon = true;
		}
	}
//SWITCHING TO PISTOL
	IEnumerator SwapPistol(){
		if (canSwitchweapon) {
			canSwitchweapon = false;
			if (usingTablet) {
				_animator.SetBool ("tabletActive", false);
				yield return new WaitForSeconds (0.5f);
				usingTablet = false;
				tablet.SetActive (false);
			}
			if (usingRifle) {
				myHand.GetComponent<Aimer> ().enabled = false;
				RHandWeight = 0;
				_canShoot = false;
				_animator.SetBool ("rifleActive", false);
				yield return new WaitForSeconds (0.7f);
				usingRifle = false;
				WeaponList [0].weaponTransform.position = rifleHolster.transform.position;
				WeaponList [0].weaponTransform.rotation = rifleHolster.transform.rotation;
				WeaponList [0].weaponTransform.parent = rifleHolster;
			}
			_animator.SetBool ("pistolActive", true);
			currentWeapon = WeaponList [1];
			yield return new WaitForSeconds (0.4f);	
			usingRifle = false;
			usingPistol = true;
			_canShoot = true;
			cursorRiffle.SetActive (false);
			cursorPistol.SetActive (true);
			cursorUnarmed.SetActive (false);
			WeaponList [1].weaponTransform.position = pistolHandHolder.transform.position;
			WeaponList [1].weaponTransform.parent = pistolHandHolder.transform;
			WeaponList [1].weaponTransform.localEulerAngles = new Vector3 (-90, 0, -90);
			yield return new WaitForSeconds (1f);
			pistolHandHolder.GetComponent<Aimer> ().enabled = true;
			_canShoot = true;
			RHandWeight = 1;
			canSwitchweapon = true;
		}
	}
	IEnumerator HidePistol(){
		if (canSwitchweapon) {
			myHand.GetComponent<Aimer> ().enabled = false;
			RHandWeight = 0;
			canSwitchweapon = false;
			_canShoot = false;
			_animator.SetBool ("pistolActive", false);
			currentWeapon = WeaponList [1];
			yield return new WaitForSeconds (0.7f);	
			usingPistol = false;
			cursorRiffle.SetActive (false);
			cursorPistol.SetActive (false);
			cursorUnarmed.SetActive (true);
			WeaponList [1].weaponTransform.position = pistolHolster.transform.position;
			WeaponList [1].weaponTransform.rotation = pistolHolster.transform.rotation;
			WeaponList [1].weaponTransform.parent = pistolHolster.transform;
			canSwitchweapon = true;
		}
	}
//SWITCHING TABLET
	IEnumerator SwapTablet(){
		if (canSwitchweapon) {
			canSwitchweapon = false;
			if (usingRifle) {
				myHand.GetComponent<Aimer> ().enabled = false;
				RHandWeight = 0;
				_canShoot = false;
				_animator.SetBool ("rifleActive", false);
				yield return new WaitForSeconds (0.7f);
				usingRifle = false;
				WeaponList [0].weaponTransform.position = rifleHolster.transform.position;
				WeaponList [0].weaponTransform.rotation = rifleHolster.transform.rotation;
				WeaponList [0].weaponTransform.parent = rifleHolster;
			}
			if (usingPistol) {
				myHand.GetComponent<Aimer> ().enabled = false;
				RHandWeight = 0;
				_canShoot = false;
				_animator.SetBool ("pistolActive", false);
				yield return new WaitForSeconds (0.7f);
				usingPistol = false;
				WeaponList [1].weaponTransform.position = pistolHolster.transform.position;
				WeaponList [1].weaponTransform.rotation = pistolHolster.transform.rotation;
				WeaponList [1].weaponTransform.parent = pistolHolster;
			}
			yield return new WaitForSeconds (0.5f);
			_animator.SetBool ("tabletActive", true);
			tablet.SetActive (true);
			usingTablet = true;
			cursorRiffle.SetActive (false);
			cursorPistol.SetActive (false);
			cursorUnarmed.SetActive (true);
			canSwitchweapon = true;
		}
	}
//HIDE TABLET
	IEnumerator HideTablet(){
		if (canSwitchweapon) {
			canSwitchweapon = false;
			_animator.SetBool ("tabletActive", false);
			yield return new WaitForSeconds (0.5f);
			usingTablet = false;
			tablet.SetActive (false);
			canSwitchweapon = true;
		}
	}
//SIT IN CHAIR
	IEnumerator SitInChair(){
		_animator.SetTrigger ("Stop");
		_collMain.enabled = false;
		canUseIK = false;
		canRotate = false;
		if(usingPistol){
			StartCoroutine(HidePistol());
		}
		if (usingRifle) {
			StartCoroutine (HideRifle ());
		}
		yield return new WaitForSeconds(0.5f);
		GameObject sitPoint = curChair.transform.Find("SitPoint").gameObject;
		this.transform.parent = sitPoint.transform;
		this.transform.position = sitPoint.transform.position;
		this.transform.rotation = sitPoint.transform.rotation;
		_animator.SetBool("sitInChair", true);
		if (curChair.GetComponent<Animator>() != null) {
			Animator chairAnim = curChair.GetComponent<Animator> ();
			chairAnim.SetBool ("sitIn", true);
		} 
		MainCam.GetComponent<MouseOrbitImproved> ().enabled = false;
		yield return new WaitForSeconds(2f);
		chairHandWeight = 1;
		GameObject seat = curChair.transform.Find("seat").gameObject;
		this.transform.position = seat.transform.position;

//IF THIS IS MARAUDER'S CHAIR
		if (curChair.transform.parent.name == "marauder") {
			curCar =curChair.transform.parent.gameObject;
			driving = true;
			curChair.transform.parent.gameObject.GetComponent<Animator> ().SetBool ("marauderOpen", true);
			SwitchCam (curCar, 20f);	
			yield return new WaitForSeconds(1f);
			_inChair = true;
		} else {
			yield return new WaitForSeconds(1f);
			_inChair = true;
			curShip = curChair.transform.root.gameObject;
			curShip.GetComponent<SpaceShipEngine> ().EngineStarted = true;
			curShip.GetComponent<SpaceShipControl> ().HideDockedCar ();
			curShip.GetComponent<SpaceShipControl> ().canControl = true;
			curShip.GetComponent<Rigidbody> ().isKinematic = false;
			curChair.GetComponent<Rigidbody> ().detectCollisions = false;
			SwitchCam (curShip, curShip.GetComponent<SpaceShipControl> ().camDist);
			if (curShip.GetComponent<SpaceShipControl> ().isUsingReflProbes) {
				curShip.GetComponent<SpaceShipControl> ().HideReflection ();
			}
		}
	}
//GET OFF CHAIR
	IEnumerator GetUpChair(){
        _inChair = false;

        if (curCar!=null) {
			curCar.GetComponent<Animator> ().SetBool ("marauderOpen", false);
			GameObject SitStartPos = curChair.transform.Find("SitPoint").gameObject;
			SwitchCamPlayer ();
			yield return new WaitForSeconds (2);
			_animator.SetBool("sitInChair", false);
			yield return new WaitForSeconds (2);
			this.transform.position = SitStartPos.transform.position;
			this.transform.rotation = SitStartPos.transform.rotation;
			_collMain.enabled = true;
			driving = false;
            chairHandWeight = 0;
            canUseIK = true;
			canRotate = true;
			this.transform.parent = null;
			curCar = null;
		}else{
            if (!curShip.GetComponent<SpaceShipControl>().landed)
            {
                curShip.GetComponent<SpaceShipControl>().AutoLevelShip();//align ship's position to the ground level;        
                yield return new WaitForSeconds(3);
            }
            curShip.GetComponent<SpaceShipControl>().canControl = false;
            if (curChair.GetComponent<Animator>() != null)
            {
                Animator chairAnim = curChair.GetComponent<Animator>();
                chairAnim.SetBool("sitIn", false);
            }
        chairHandWeight = 0;
        _animator.SetBool("sitInChair", false);        
		curShip.GetComponent<Rigidbody> ().isKinematic = true;
		curChair.GetComponent<Rigidbody> ().detectCollisions = true;
		SwitchCamPlayer ();
		MainCam.transform.SetParent(null);
		if (curShip.GetComponent<SpaceShipControl> ().isUsingReflProbes) {
			curShip.GetComponent<SpaceShipControl> ().ShowReflection();
		}
		curShip.GetComponent<SpaceShipControl> ().ShowDockedCar();
		curShip.GetComponent<SpaceShipEngine>().EngineStarted = false;
		yield return new WaitForSeconds(1f);
		MainCam.transform.SetParent(this.transform);
		GameObject sitPoint = curChair.transform.Find("SitPoint").gameObject;
		this.transform.parent = null;
		_collMain.enabled = true;
		transform.position = sitPoint.transform.position;
		canRotate = true;		
		canUseIK = true;
        curShip.GetComponent<SpaceShipControl>().ResetAutoLevel();//align ship's position to the ground level;
        }
	}
//GET IN THE CAR
	IEnumerator GetInCar(){
		Animator CarAnim = curCar.GetComponent<Animator> ();
		_collMain.enabled = false;
		canUseIK = false;
		canRotate = false;
		_animator.SetTrigger ("Stop");
		if(usingPistol){
			StartCoroutine(HidePistol());
			yield return new WaitForSeconds (2);
		}
		if(usingRifle){
			StartCoroutine(HideRifle());
			yield return new WaitForSeconds (2);
		}
		yield return new WaitForSeconds(0.5f);
		this.transform.parent = curCar.transform;
		if (_carRightDoor != null) {
			GameObject SitStartPos = curCar.transform.Find("sitInCarPosition").gameObject;
			CarAnim.SetTrigger ("rightDoor");
			this.transform.position = SitStartPos.transform.position;
			this.transform.rotation = SitStartPos.transform.rotation;
			_animator.SetBool ("SitInCarRight", true);

		}
		if (_carLeftDoor != null) {
			GameObject SitStartPos = curCar.transform.Find("sitInCarPositionL").gameObject;
			CarAnim.SetTrigger ("leftDoor");
			this.transform.position = SitStartPos.transform.position;
			this.transform.rotation = SitStartPos.transform.rotation;
			_animator.SetBool ("SitInCarLeft", true);
		}
		MainCam.GetComponent<MouseOrbitImproved> ().enabled = false;
		yield return new WaitForSeconds (4);
		driving = true;
		GameObject carSeat = curCar.transform.Find("CarChairPosition").gameObject;
		this.transform.position = carSeat.transform.position;
		this.transform.rotation = carSeat.transform.rotation;        
		SwitchCam(curCar, 20f);
	}
//GET OUT THE CAR
	IEnumerator GetOutCar(){
		Animator CarAnim = curCar.GetComponent<Animator> ();
		CarAnim.SetTrigger ("OpenFromInside");
		_animator.SetBool ("SitInCarRight", false);
		_animator.SetBool ("SitInCarLeft", false);
		driving = false;
		GameObject SitStartPos = curCar.transform.Find("sitInCarPosition").gameObject;
        SwitchCamPlayer ();
		yield return new WaitForSeconds (4);
		this.transform.position = SitStartPos.transform.position;
		this.transform.rotation = SitStartPos.transform.rotation;
		_collMain.enabled = true;
		canUseIK = true;
		canRotate = true;
		this.transform.parent = null;
	}
//GET IN SPACESHIP
	IEnumerator GetInShip(){
		canUseIK = false;
		canRotate = false;
		_animator.SetTrigger ("Stop");
		if(usingPistol){
			StartCoroutine(HidePistol());
			yield return new WaitForSeconds (2);
		}
		if(usingRifle){
			StartCoroutine(HideRifle());
			yield return new WaitForSeconds (2);
		}
		_collMain.enabled = false;
		this.transform.parent = curShip.transform;
		GameObject sitPoint = curShip.transform.Find("SitPointR").gameObject;
		this.transform.position = sitPoint.transform.position;
		this.transform.rotation = sitPoint.transform.rotation;
		_animator.SetBool("GetInShipR", true);
		yield return new WaitForSeconds(3f);
		GameObject seat = curShip.transform.Find("seat").gameObject;
		this.transform.position = seat.transform.position;
		this.transform.rotation = seat.transform.rotation;
		curShip.GetComponent<SpaceShipControl> ().canControl = true;
		curShip.GetComponent<SpaceShipEngine>().EngineStarted = true;
		curShip.GetComponent<Rigidbody> ().isKinematic = false;
		SwitchCam(curShip, 20f);
	}
//GET OUT SPACESHIP
	IEnumerator GetOutShip(){
		_animator.SetBool("GetInShipR", false);
		curShip.GetComponent<SpaceShipEngine>().EngineStarted = false;
		curShip.GetComponent<SpaceShipControl> ().canControl = false;
		curShip.GetComponent<Rigidbody> ().isKinematic = true;
		SwitchCamPlayer ();
		yield return new WaitForSeconds(3f);
		GameObject sitPoint = curShip.transform.Find("SitPointR").gameObject;
		this.transform.parent = null;
		_collMain.enabled = true;
		transform.position = sitPoint.transform.position;
		canRotate = true;		
		canUseIK = true;
	}
//GET ON HOVER
	IEnumerator GetOnHover(){
		canUseIK = false;
		canRotate = false;
		_animator.SetTrigger ("Stop");
		if(usingPistol){
			StartCoroutine(HidePistol());
			yield return new WaitForSeconds (2);
		}
		if(usingRifle){
			StartCoroutine(HideRifle());
			yield return new WaitForSeconds (2);
		}
		_collMain.enabled = false;
		this.transform.SetParent(curHover.transform);
		GameObject sitPoint = curHover.transform.Find("SitPoint").gameObject;
		this.transform.position = sitPoint.transform.position;
		this.transform.rotation = sitPoint.transform.rotation;
		_animator.SetBool("GetInHover", true);
		yield return new WaitForSeconds(5f);
		this.GetComponent<HandPlacement>().LeftHandPos = curHover.GetComponent<Hover>().LHandPos;
		this.GetComponent<HandPlacement>().RightHandPos = curHover.GetComponent<Hover>().RHandPos;
		this.GetComponent<HandPlacement>().handIk = true;
		GameObject seat = curHover.transform.Find("seat").gameObject;
		this.transform.position = seat.transform.position;
		this.transform.rotation = seat.transform.rotation;
		curHover.GetComponent<Rigidbody> ().isKinematic = false;
		curHover.GetComponent<Hover> ().canControl = true;
		SwitchCam(curHover, 10f);
	}
//Get Off hover
	IEnumerator GetOffHover(){
		_animator.SetBool("GetInHover", false);
		curHover.GetComponent<Rigidbody> ().drag = 5;
		curHover.GetComponent<Hover> ().canControl = false;
		SwitchCamPlayer ();
		this.GetComponent<HandPlacement>().handIk = false;
		yield return new WaitForSeconds(5f);
		GameObject sitPoint = curHover.transform.Find("SitPoint").gameObject;
		curHover.GetComponent<Rigidbody> ().isKinematic = true;
		this.transform.parent = null;
		_collMain.enabled = true;
		transform.position = sitPoint.transform.position;
		canRotate = true;		
		canUseIK = true;
	}
//GET ON MECH
	IEnumerator GetOnMech(){
		canUseIK = false;
		canRotate = false;
		_animator.SetTrigger ("Stop");
		if(usingPistol){
			StartCoroutine(HidePistol());
			yield return new WaitForSeconds (2);
		}
		if(usingRifle){
			StartCoroutine(HideRifle());
			yield return new WaitForSeconds (2);
		}
		_collMain.enabled = false;
		yield return new WaitForSeconds (0.3f);
		this.transform.SetParent(curMech.GetComponent<MechControll>().sitParent.transform);
		GameObject sitPoint = curMech.transform.Find("SitPoint").gameObject;
		this.transform.position = sitPoint.transform.position;
		this.transform.rotation = sitPoint.transform.rotation;
		_animator.SetBool("getInMech", true);
		yield return new WaitForSeconds(6.5f);
		this.GetComponent<HandPlacement>().LeftHandPos = curMech.GetComponent<MechControll>().LHandPos;
		this.GetComponent<HandPlacement>().RightHandPos = curMech.GetComponent<MechControll>().RHandPos;
		this.GetComponent<HandPlacement>().handIk = true;
		GameObject seat = curMech.transform.Find("seat").gameObject;
		this.transform.position = seat.transform.position;
		this.transform.rotation = seat.transform.rotation;
		curMech.GetComponent<MechControll> ().activated = true;
		yield return new WaitForSeconds(1f);
		curMech.GetComponent<MechControll> ().canControll = true;
		SwitchCam(this.gameObject, 10f);
	}
//GET OFF MECH
	IEnumerator GetOffMech(){
		_animator.SetBool("getInMech", false);
		curMech.GetComponent<MechControll> ().canControll = false;
		curMech.GetComponent<MechControll> ().activated = false;
		SwitchCamPlayer ();
		this.GetComponent<HandPlacement>().handIk = false;
		yield return new WaitForSeconds(6f);
		GameObject sitPoint = curMech.transform.Find("SitPoint").gameObject;
		this.transform.parent = null;
		_collMain.enabled = true;
		transform.position = sitPoint.transform.position;
		canRotate = true;		
		canUseIK = true;
	}
//CAMERA SWITCHER
	void SwitchCam(GameObject camTarget, float camDist){
		if(curHover == null && curCar ==null && curMech==null){
            Camera.main.transform.SetParent(null);
			MainCam.GetComponent<MouseOrbitSimple> ().enabled = true;
			MainCam.GetComponent<SmoothFollow> ().enabled = false;
			MainCam.GetComponent<MouseOrbitImproved> ().enabled = false;
			MainCam.GetComponent<MouseOrbitSimple>().PlayerCamTarget = curShip.transform.Find("camTarget");
		}else{
		MainCam.GetComponent<SmoothFollow> ().enabled = true;
		MainCam.GetComponent<SmoothFollow> ().target = camTarget.transform;
		MainCam.GetComponent<SmoothFollow> ().distance = camDist;
		MainCam.GetComponent<MouseOrbitImproved> ().enabled = false;
            if (curCar || curMech) {
                MainCam.transform.SetParent(null);
            }
		}
	}
	void SwitchCamPlayer(){
        MainCam.transform.SetParent(this.transform);
        MainCam.GetComponent<SmoothFollow> ().enabled = false;
		MainCam.GetComponent<MouseOrbitImproved> ().enabled = true;
		MainCam.GetComponent<MouseOrbitSimple> ().enabled = false;
	}
//FOOTSTEP SOUND FUNCTION IS CALLED FROM ANIMATION EVENT 
	void footStep(){
		if (isGrounded () && _collMain.enabled==true) {	
//SHOOTING RAY DOWN
			Vector3 bottom = -transform.up;
			Vector3 btmRayStart = new Vector3(this.transform.position.x, this.transform.position.y+1, this.transform.position.z);
			Ray	btmRay = new Ray(btmRayStart, bottom);
			RaycastHit hit;
			Debug.DrawRay(btmRayStart, transform.forward, Color.green);
			if(Physics.Raycast(btmRay, out hit, 2f)){
				if(hit.transform.gameObject.layer == 9){
					int n = Random.Range (1, m_MetalFootstepSounds.Length);
					m_AudioSource.clip = m_MetalFootstepSounds [n];
					m_AudioSource.PlayOneShot (m_AudioSource.clip);
					//			move picked sound to index 0 so it's not picked next time
					m_MetalFootstepSounds [n] = m_MetalFootstepSounds [0];
					m_MetalFootstepSounds [0] = m_AudioSource.clip;
				}else{
					int n = Random.Range (1, m_FootstepSounds.Length);
					m_AudioSource.clip = m_FootstepSounds [n];
					m_AudioSource.PlayOneShot (m_AudioSource.clip);
					//			move picked sound to index 0 so it's not picked next time
					m_FootstepSounds [n] = m_FootstepSounds [0];
					m_FootstepSounds [0] = m_AudioSource.clip;
				}
			}
		}
	}
	void jumpSound(){
		if (_collMain.enabled) {
			m_AudioSource.clip = m_jumpSound;
			m_AudioSource.PlayOneShot (m_AudioSource.clip);
		}
	}
	public void ReceiveDamage(float demAmmount){//function called by enemie's ray hitting any of the player's collider
		health -= demAmmount;
		int n = Random.Range (1, m_BodyHit.Length);
		m_AudioSource.clip = m_BodyHit [n];
		m_AudioSource.Play();
		underFire = true;
		this.GetComponent<BloodEffect> ().waitTime = 5;//resetting delay of player's health restore check BloodEffect script
		if (health <= 0) {
			Die();
		}
	}
	public void Die() {//dieing function (turning character to ragdoll)		
		canRotate = false;
		_collMain.enabled = false;//disable character controller
		_animator.enabled = false;//disable animation component
		alive = false;
		foreach (Rigidbody rb in rigidBodies){
			rb.isKinematic = false;//make all character's rigidbodies nonkinematic
		}
		foreach (Collider col in ragDallColliders){
			col.enabled = true;//lets make all the colliders of the character disabled
		}
	}
}

