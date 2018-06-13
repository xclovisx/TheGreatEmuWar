using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent agent;

	public Collider [] ragDallColliders;//array of character's collider
	public Component[] rigidBodies;//array of character's rigidbodies
	public AudioClip[] aggroSound;
	public AudioClip[] hitSound;
	public AudioClip[] m_FootstepSoundsGround;
	public AudioClip CPBip;
	public AudioClip ShutDown;
	public AudioClip gunShot;
	private AudioSource m_AudioSource;
	public AudioSource gunASource;
	public Transform[] waypoint;//array of patroll waypoints
	public Transform cratePos;
	public Transform gunTip;
	public Vector3 lastKnownPos;
	private float attackTime = 1;
	private float attackTimeReset = 1;
	public float health;//health count
	public float idlingTime = 3f;//time we're going to spend for idling
	public float idleTimeReset = 3f;//idlinf time reseter
	public float walkRadius = 20f;
	private int speed = 1;
	public int currentWaypoint;//current waypoint we're heading to	
	public float timer = 1f;//time we're going to spend searching player
	private float timeCheck;//

	public GameObject electricSparks;
	public GameObject weldingParticles;
	public GameObject WeldingSpot;
	public GameObject curCP;
	public GameObject curCrate;
	private bool picking; 
	private bool attacking;
	public bool canRest;
	public bool aggressive;
	public bool alive = true;//i am alive
	public bool idling = false;//i am idling
	private EnemySences mySences;
	private playerControl playerScript;
	public enum State {//enumeration of states
		Idle,
		Patrol,
		Attack,
		Loader,
		CPInteraction,
		Welding,
		CheckLastPos,
	}
	public State state;
	private Animator _animator;
	private GameObject Player;//Player

	private Vector3 target;//coordinates of the target
	private Vector3 moveDirection;//movement direction
	private Vector3 lookDirection;//look direction

	void Awake () {		
///////CACHING VARIABLES
		_animator = GetComponent<Animator> ();
		mySences = this.GetComponent<EnemySences>();
		agent = (UnityEngine.AI.NavMeshAgent)this.GetComponent("NavMeshAgent");
		Player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		playerScript = Player.GetComponent<playerControl>();
		rigidBodies = GetComponentsInChildren<Rigidbody>();//our character is a ragdoll, lets collect its rigidbodies
		curCP = FindClosestCP();

		state = Enemy.State.Idle;
		StartCoroutine("FSMach");//start our state machine	
		m_AudioSource = GetComponent<AudioSource>();
			foreach (Rigidbody rb in rigidBodies){//lets make all reigidbodies of the character kinematic
			rb.isKinematic = true;
		}
		foreach (Collider col in ragDallColliders){
			col.enabled = false;//lets make all the colliders of the character disabled
		}		
	}
	private IEnumerator FSMach(){
		while(alive){//if character is still alive
			switch(state){
			case State.Patrol:
				Patrol();
				break;
			case State.Attack:
				Attack();
				break;
			case State.Idle:
				Idle();
				break;
			case State.Loader:
				Loader ();
				break;
			case State.CPInteraction:
				CPInteraction ();
				break;
			case State.Welding:
				Welding ();
				break;
			case State.CheckLastPos:
				CheckLastPos ();
				break;
			}
			yield return null;
		}
	}
	private void Patrol(){//patrol state
		Moving();
		speed=1;//if we're patroling lets move slowly
		agent.speed = 0.1f;
		if(mySences.playerInSight && aggressive && playerScript.alive){
			idling = false;//we're not idling anymore
			state = Enemy.State.Attack;
		}
		if(currentWaypoint < waypoint.Length) {//if current waypoint's number is lesser than the length of waypoint array's length
			target = waypoint[currentWaypoint].position;//our target is current waypoint
			moveDirection = target - this.transform.position;//calculate move direction (distance between current waypoint and character)
			if(moveDirection.magnitude < 1.5f){//if we're to close to current waypoint
				currentWaypoint++;//increase current waypoint's number
			}
		}
		else{
			currentWaypoint=0;//don't make current waypoint number greater than length of waypoint array
		}
		if (Time.time - timeCheck > Random.Range(5, 10)) {
			timeCheck = Time.time;//time check equal to absolute time
			timer++;//increase timer by 1
		}
		if(timer>Random.Range(2, 10)){//while timer is lesser than 5
			state = Enemy.State.Idle; // changing state to patrol
			timer=0;//zeroing timer
		}
	}
	private void Attack(){//attack state
		idling = false;//we're not idling anymore
		speed=2;//change movement speed from 0 to 2
		agent.speed = 0;
		attacking = true;
		target = Player.transform.position; // Player is current target
		Moving(); //run movement function
		if(!mySences.playerInSight){//player is not visible			
			lastKnownPos = Player.transform.position;
			state = Enemy.State.CheckLastPos;//scheck last pos
		}
		if(((Player.transform.position - this.transform.position).magnitude < 10f)){//if distance to player is lesser than 2 or character is hitted
			speed=0;//stop movement
			agent.speed = 0;
			lookDirection = target - this.transform.position;  // calculating look  direction (towards target)
			lookDirection.y = 0; // restricting Y axis rotation
			Quaternion newRot = Quaternion.LookRotation (lookDirection);//rotation that looks along forward with the the head upwards along upwards
			this.transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * 15); //smooth linear interpolation from our current rotation to new rotation

		}
		else{//if distance to player is bigger than 2 and character is not hitted
			speed=2;//move character faster
			agent.speed = 1f;
		}
		if(((Player.transform.position - this.transform.position).magnitude > 100f)){//if player escaped from character (distance between tham greater than 20)
			lastKnownPos = Player.transform.position;
			state = Enemy.State.CheckLastPos;//scheck last pos
		}
		if (attackTime > 0){//while attack time is greater than zero 
			attackTime -= Time.deltaTime;//descrease attack timer by Time.deltaTime
		}
		if (attackTime < 0){ 
			attackTime = 0; //don't make attack timer lesser than 0
		}
		if (attackTime == 0){//when attack timer became zero
			attackTime = attackTimeReset;//lets reset attack timer
			_animator.SetTrigger("Shoot");
		}
		if (!playerScript.alive) {
			state = Enemy.State.Idle;
		}
	}
//IDLE STATE
	private void Idle(){//idling state
		speed = 0;
		agent.speed = 0;
		idling = true;
		attacking = false;
		if(idling){
			if (idlingTime > 0)//while idling time is greater than 0
				idlingTime -= Time.deltaTime;//descrease idling time by Time.deltaTime
			if(mySences.playerInSight && aggressive && playerScript.alive){
					idling = false;//we're not idling anymore
					state = Enemy.State.Attack;
				}
			if (idlingTime < 0) 
				idlingTime = 0;//idling time can not be lesser than 0
			if (idlingTime == 0)//when idling time become 0
			{
				if (!aggressive) {
					idling = false;////we're not idling anymore
					idleTimeReset = Random.Range (5, 10);
					idlingTime = idleTimeReset;//reset our idling timer
					speed = 1;//change movement speed from 0 to 2
					int RandNum = Random.Range (1, 5);
					if (RandNum == 1) {
						state = Enemy.State.Patrol;//we're patroling
					}
					if (RandNum == 2) {
						curCrate = FindClosestCrate ();
						if (curCrate != null) {
							state = Enemy.State.Loader;
						} else {
							state = Enemy.State.Patrol;//we're patroling
						}
					}
					if (RandNum == 3) {
						curCP = FindClosestCP ();
						state = Enemy.State.CPInteraction;
					}
					if (RandNum == 4) {
						target = WeldingSpot.transform.position;
						state = Enemy.State.Welding;
					}
				} else {
					idling = false;////we're not idling anymore
					idleTimeReset = Random.Range (5, 10);
					idlingTime = idleTimeReset;//reset our idling timer
					speed = 1;//change movement speed from 0 to 2
					state = Enemy.State.Patrol;//we're patroling
				}
			}
		}
	}
//CHECK PLAYER'S LAST KNOWN POSITION
	private void CheckLastPos(){
		idling = false;
		target = lastKnownPos;
		Moving();
		speed=1;//if we're patroling lets move slowly
		agent.speed = 0.1f;
		float distToPos = Vector3.Distance(this.transform.position, target);
		if(distToPos < 1f){
			idling = true;//we're not idling anymore
			state = Enemy.State.Idle;
		}
		if(mySences.playerInSight && aggressive && playerScript.alive){
			idling = false;//we're not idling anymore
			state = Enemy.State.Attack;
		}
	}
//CRATE INTERACTION
	private void Loader(){
		idling = false;
		if(curCrate.transform.parent==null){
			target = curCrate.transform.position;
		}
		Moving();
		speed=1;
		agent.speed = 0.1f;
		if(!picking){
			float distToCrate = Vector3.Distance(this.transform.position, curCrate.transform.position);
			if(distToCrate < 2.5f){
				_animator.SetBool("Pickup", true);
				picking = true;
				agent.speed = 0;
				speed = 0;
			}
		}
		if(curCrate.transform.parent!=null){
			if(((this.transform.position - target).magnitude < 5f)){//
				_animator.SetBool("PutDown", true);
				_animator.SetBool("HoldingCrate", false);
				agent.speed = 0;
			}
		}
	}
//CONTROL PANEL INTERACTION 
	private void CPInteraction(){
		idling = false;
		target = curCP.transform.Find ("cpAligner").gameObject.transform.position; // Player is current target
		Moving();
		speed=1;//if we're patroling lets move slowly
		agent.speed = 0.1f;
		if(((this.transform.position - target).magnitude < 1.5f)){
			speed = 0;
			agent.speed = 0;
			this.transform.position = Vector3.Slerp (this.transform.position, curCP.transform.Find ("cpAligner").gameObject.transform.position, Time.deltaTime * 5);
			this.transform.rotation = Quaternion.Lerp (transform.rotation, curCP.transform.Find ("cpAligner").gameObject.transform.rotation, Time.deltaTime * 5);
			_animator.SetBool("InteractCP", true);
		}
	}

//PROCESS MOVEMENT
	private void Moving(){//movement function
		if(!idling){
			agent.SetDestination(target);
		}
	}
	void FixedUpdate(){
		if(idling){
			agent.Stop();
		}
		else{
			agent.Resume();
		}
		if (health <= 0) {
			if(alive){
				m_AudioSource.PlayOneShot (ShutDown);
				electricSparks.SetActive (true);
			}
			alive = false;
			Die ();
		}
		_animator.SetBool ("Aggressive", aggressive);
		_animator.SetBool ("Attacking", attacking);
		_animator.SetInteger ("Speed", speed);
	}
//WELDING
	private void Welding(){
		idling = false;
		Moving();
		speed=1;//if we're patroling lets move slowly
		agent.speed = 0.1f;
		if(((this.transform.position - target).magnitude <= 2f)){//
			speed = 0;
			agent.speed = 0;
			this.transform.position = Vector3.Slerp (this.transform.position, WeldingSpot.transform.position, Time.deltaTime * 5);
			this.transform.rotation = Quaternion.Lerp (transform.rotation, WeldingSpot.transform.rotation, Time.deltaTime * 5);
			_animator.SetBool("Welding", true);
		}
	}
	public void WithCrate(){
		curCrate.GetComponent<Rigidbody>().isKinematic=true;
		curCrate.transform.position = cratePos.position;
		curCrate.transform.rotation = cratePos.rotation;
		curCrate.transform.parent = cratePos;
		speed = 1;
		agent.speed = 0.1f;
		_animator.SetBool("Pickup", false);
		_animator.SetBool("HoldingCrate", true);

		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
		randomDirection += transform.position;
		UnityEngine.AI.NavMeshHit hit;
		UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
		Vector3 finalPosition = hit.position;
		target = finalPosition;
	}
	public void PutCrateDown(){
		curCrate.transform.parent = null;
		curCrate.GetComponent<Rigidbody>().isKinematic=false;
		picking = false;
		_animator.SetBool("PutDown", false);
		state = Enemy.State.Idle;//switch state to Idle 
		speed = 1;
		agent.speed = 0.1f;
	}
	public void FinishCPIntraction(){
		_animator.SetBool("InteractCP", false);
		curCP = null;
		state = Enemy.State.Idle;//switch state to Idle 
	}
	public void Die() {//dieing function (turning character to ragdoll)
		agent.Stop();
		alive = false;
		_animator.enabled = false;//disable animation component
		foreach (Collider col in ragDallColliders){
			col.enabled = true;//turn on all colliders of the character
		}
		foreach (Rigidbody rb in rigidBodies){
			rb.isKinematic = false;//make all charcters rigidbodies nonkinematic
		}
	}
	GameObject FindClosestCrate() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("crate");
		GameObject closest = null;
		float distance = 200f;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
	GameObject FindClosestCP() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("controlPanel");
		GameObject closestCP = null;
		float distance = 1000f;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closestCP = go;
				distance = curDistance;
			}
		}
		return closestCP;
	}
	void AlertBuddies(){//lets find closest buddies and change their states to attack
		GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag ("enemy");
		List<GameObject> ClosestEnemies = new List<GameObject> ();
		foreach (GameObject node in AllEnemies) {
			float dist = (Player.transform.position - node.transform.position).magnitude;
			if(dist < 100)
			{
				ClosestEnemies.Add (node);
				node.GetComponent<Enemy> ().state = Enemy.State.Attack;
			}
		}
	}
	//this function is executed by animation event in zombie attack animation clip;
	public void ApplyDamage(float dmg){		
		health -= dmg;	
		idling = false;//we're not idling anymore
		lastKnownPos =  Player.transform.position;
		AlertBuddies ();
		state = Enemy.State.Attack;

	}
	//FOOTSTEP SOUND FUNCTION IS CALLED FROM ANIMATION EVENT
	void footStep(){
		int n = UnityEngine.Random.Range (0, m_FootstepSoundsGround.Length);
		m_AudioSource.clip = m_FootstepSoundsGround [n];
		m_AudioSource.PlayOneShot (m_AudioSource.clip);
		// move picked sound to index 0 so it's not picked next time
		m_FootstepSoundsGround [n] = m_FootstepSoundsGround [0];
		m_FootstepSoundsGround [0] = m_AudioSource.clip;				
	}
	void CPSound(){
		m_AudioSource.PlayOneShot (CPBip);
	}
	public void StartWelding(){
		weldingParticles.SetActive (true);
	}
	public void StopWelding(){
		weldingParticles.SetActive (false);
	}
	public void FinishWelding(){
		state = Enemy.State.Idle;//switch state to Idle 
		_animator.SetBool ("Welding", false);
	}
	public void ShootRay(){ //called by animation event
		Vector3 direction = ( (Player.transform.position + transform.up*Random.Range(0f,3.5f) + transform.right*Random.Range(-.7f,.7f)) - gunTip.position ).normalized;//added 2.5 offset because players pivot is at 0
		Ray ray = new Ray( gunTip.position, direction );
		RaycastHit hit;
		Debug.DrawRay (gunTip.position, direction*10, Color.red, 100);
		gunASource.PlayOneShot (gunShot);
		if(Physics.Raycast(ray, out hit, 100f)){
			if (hit.collider.gameObject.tag == "Player") {
				hit.collider.gameObject.SendMessage ("ReceiveDamage", 20);//send message to player script;
			}
		}
	}
}
