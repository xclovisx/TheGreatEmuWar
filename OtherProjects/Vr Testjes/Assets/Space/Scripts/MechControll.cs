using UnityEngine;
using System.Collections;

public class MechControll : MonoBehaviour {
	private Animator _animator;
	private AnimatorStateInfo currentBaseState;
	private CharacterController _charCtrl;
	private AudioSource m_AudioSource;
	public SmoothFollow camScrpt;
	public AudioClip m_Step;
	public AudioClip m_MechUp;
	public AudioClip m_Landing;
	public AudioClip m_Jump;

	private Vector3 moveDirection;//movement direction
	public GameObject sitParent;
	public GameObject bulletPrefab;
	public Transform gun;
	public Transform RHandPos;
	public Transform LHandPos;

	public bool activated;
	public bool canControll;
	private LayerMask characterMask = 5;
	private bool run;
	private float _speed;
	private float _strafe;
	public float walkSpeed = 2f;
	public float jumpSpeed;
	public float rotationSpeed = 60f;//speed of rotating
	private float gravity = 4.0f;//gravity affecting character
	private float speed = 5.0f;//character's movement speed

	//ANIMATION STATES
	static int jumpState = Animator.StringToHash("Base Layer.MechMidAir");
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_charCtrl = GetComponent<CharacterController>();
		m_AudioSource = GetComponent<AudioSource>();
		camScrpt = Camera.main.GetComponent<SmoothFollow>();
	}
	
	// Update is called once per frame
	void Update () {
		if(canControll){		

			if(Input.GetMouseButton(0)){
				_animator.SetTrigger("Twirl");
			}
		_speed = Input.GetAxis("Vertical");//reading vertical axis input
		_strafe = Input.GetAxis("Horizontal");
		if(Input.GetAxis("Vertical") > 0) {
			_charCtrl.Move(transform.forward *walkSpeed *Time.deltaTime);
			}
		if(Input.GetAxis("Vertical") < 0) {
			_charCtrl.Move(transform.forward *-walkSpeed *Time.deltaTime);
			}
//////////ROTATING RIGHT
		if(Input.GetAxis("Horizontal") > 0) {
			transform.Rotate(0, Input.GetAxis("Horizontal")*rotationSpeed *Time.deltaTime, 0);
			}
//////////ROTATING LEFT
		if(Input.GetAxis("Horizontal") < 0) {
			transform.Rotate(0, Input.GetAxis("Horizontal")*rotationSpeed *Time.deltaTime, 0);
			}
			if(Input.GetKey(KeyCode.LeftShift) && _speed>0	&& !_animator.GetBool("jump")){
				run=true;
				walkSpeed = 10;
			}
			else{
				run=false;
			}
			if(!run && !_animator.GetBool("jump")){
				walkSpeed = 5;
				rotationSpeed = 90;
			}
			if (_animator.GetCurrentAnimatorStateInfo(0).IsName("MechLanding")){
				rotationSpeed = 0;
				walkSpeed = 0;
			}
			_charCtrl.Move(moveDirection *speed *Time.deltaTime); //moving character controller forward with speed over time
			moveDirection.y -= gravity * Time.deltaTime;// applying gravity
		}
		else{
			_speed = 0;
			_strafe = 0;
		}
//Applying Gravity
		if(isGrounded()&& canControll){
			if (Input.GetButton ("Jump")) {	
				walkSpeed = 0;
				rotationSpeed = 0;
				_animator.SetTrigger ("jump");
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		if(_charCtrl.enabled){
			_charCtrl.Move (moveDirection * Time.deltaTime);
			if(moveDirection.y < -9f){
				moveDirection.y = -9f;
			}
		}

		if(currentBaseState.fullPathHash == jumpState){
			_animator.ResetTrigger("jump");
		}

	}
	void FixedUpdate(){
		_animator.SetBool("run", run);
		_animator.SetFloat("Speed", _speed);
		_animator.SetFloat("Strafe", _strafe);
		_animator.SetBool("controll", activated);
		_animator.SetBool("Grounded", isGrounded());
		_animator.SetFloat ("VertVelocity", _charCtrl.velocity.y);
		currentBaseState = _animator.GetCurrentAnimatorStateInfo(0);
	}
	bool isGrounded()	{
		return Physics.CheckSphere(transform.position, 1f, characterMask | 1<<9);
	}
	IEnumerator Jump(){
		rotationSpeed = 0;
		walkSpeed = 0;
		yield return new WaitForSeconds(0.1f);
		moveDirection.y = jumpSpeed;
		rotationSpeed = 90;
		walkSpeed = 5;
	}
	IEnumerator Stop(){
		yield return new WaitForSeconds(0.3f);
		rotationSpeed = 90;
		walkSpeed = 5;
	}
//FOOTSTEP SOUND FUNCTION IS CALLED FROM ANIMATION EVENT 
	void footStep(){
		if (isGrounded ()) {	
			m_AudioSource.PlayOneShot (m_Step);
			camScrpt.shake = 0;
		}
	}
	void MechUp(){
		if (isGrounded ()) {	
			m_AudioSource.PlayOneShot (m_MechUp);
		}
	}
	void MechLanding(){
		if (isGrounded ()) {	
			m_AudioSource.PlayOneShot (m_Landing);
			camScrpt.shake = 0;
		}
	}
	void MechJump(){
		if (isGrounded ()) {	
			m_AudioSource.PlayOneShot (m_Jump);
		}
	}
}
