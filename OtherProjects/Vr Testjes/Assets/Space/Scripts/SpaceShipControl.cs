using UnityEngine;
using System.Collections;

public class SpaceShipControl : MonoBehaviour {
	public float MaxEnginePower = 40;
	public float RollEffect = 1;
	public float PitchEffect = 50;
	public float YawEffect = 0.2f;
	public float BankedTurnEffect = 0.5f;
	public float AutoTurnPitch = 0.5f;
	public float AutoRollLevel = 0.1f;
	public float AutoPitchLevel = 0.1f;
	public float AirBreaksEffect = 3f;
	public float ThrottleChangeSpeed = 0.3f;
	public float DragIncreaseFactor = 0.001f;
  	public float turningAmmount = 30f;
	public float spaceRotAmmount =2f;
	public float shipRayOffset;
	public float camDist;
	public GameObject shipReflProbes;
	public float hyperDriveEnginePower = 1200;
	public float MaxEnginePowerVar = 400;

    public ParticleSystem starDust;

	private bool AirBrakes;
	public bool landed;
	public bool mouseControled;
	private float Throttle;
	private float ForwardSpeed;
	private float EnginePower;
	private float cur_MaxEnginePower;
	private float RollAngle;
	private float PitchAngle;
	private float RollInput;
	private float PitchInput;
	private float YawInput;
	private float ThrottleInput;

	private float OriginalDrag;
	private float OriginalAngularDrag;
	private float AeroFactor = 1;
	private bool Immobilized = false;
	private float BankedTurnAmount;
	private Rigidbody rigidBody;
	private Animator _anim;
	private GameObject Player;
	private playerControl PlayerScrpt;

	public bool isUsingReflProbes;
	public bool canControl;
	public ParticleSystem[] enginesParticles;
	public GameObject dockedCar;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		PlayerScrpt = Player.GetComponent<playerControl>();
		starDust = Camera.main.transform.Find("Particle").GetComponent<ParticleSystem>();

		rigidBody = this.GetComponent<Rigidbody>();
		_anim = this.GetComponent<Animator>();
		OriginalDrag = rigidBody.drag;
		OriginalAngularDrag = rigidBody.angularDrag;	
	}
	
	// Update is called once per frame
	void Update(){
        //HYPERDRIVE
        if (canControl)
        {
            if (Input.GetKey(KeyCode.Tab) && EnginePower == MaxEnginePower && PlayerScrpt.inSpace)
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 120, Time.deltaTime * 10);
                MaxEnginePower = hyperDriveEnginePower;
                starDust.GetComponent<ParticleSystemRenderer>().renderMode = ParticleSystemRenderMode.Stretch;
                ParticleSystem.InheritVelocityModule inheritVelocity = starDust.inheritVelocity;
                inheritVelocity.mode = ParticleSystemInheritVelocityMode.Initial;
                ParticleSystem.EmissionModule emissionModule = starDust.emission;
                emissionModule.rateOverDistanceMultiplier =0.1f;
                hyperdrive();
            }
            else {
                starDust.GetComponent<ParticleSystemRenderer>().renderMode = ParticleSystemRenderMode.Billboard;
                ParticleSystem.InheritVelocityModule inheritVelocity = starDust.inheritVelocity;
                inheritVelocity.mode = ParticleSystemInheritVelocityMode.Current;
                ParticleSystem.EmissionModule emissionModule = starDust.emission;
                emissionModule.rateOverDistanceMultiplier = 1f;
                MaxEnginePower = MaxEnginePowerVar;
                if (Camera.main.fieldOfView != 60)
                {
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 5f);
                    if (Camera.main.fieldOfView < 61)
                    {
                        Camera.main.fieldOfView = 60;
                    }
                }
            }
        }
	}
	public void Move (float rollInput, float pitchInput, float yawInput, float throttleInput, bool airBrakes) {       
            this.RollInput = rollInput;
            this.PitchInput = -pitchInput;//invert horizontal rotation
            this.YawInput = yawInput;
            this.ThrottleInput = throttleInput;
            this.AirBrakes = airBrakes;

            if (canControl)
            {
                ClampInput();
                CalculateRollAndPitchAngles();
                AutoLevel();
                CalculateForwardSpeed();
                ControlThrottle();
                CalculateDrag();
                CalculateLinearForces();
                CalculateTorque();
                foreach (ParticleSystem engineTrace in enginesParticles)
                {
                    engineTrace.startLifetime = 0.2f + ThrottleInput / 2;
                }
                if (PlayerScrpt.inSpace)
                {
                    ParticleSystem.EmissionModule em = starDust.emission;
                    em.enabled = true;
                }
                else {
                    ParticleSystem.EmissionModule em = starDust.emission;
                    em.enabled = false;
                }
            }
            if (Input.GetKey(KeyCode.C))
            {
                rigidBody.AddForce(transform.up * Time.deltaTime * 1500 * rigidBody.mass);
                if (landed)
                {
                    rigidBody.useGravity = false;
                    canControl = true;
                    landed = false;
                    _anim.SetBool("landing", false);
                    _anim.SetBool("departure", true);
                }
            }
            if (!landed)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    rigidBody.AddTorque(transform.forward * 500* RollEffect);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    rigidBody.AddTorque(transform.forward * -500* RollEffect);
                }
            }

            if (Throttle < 0.1f)
            {
                Vector3 currentVelocity = rigidBody.velocity;
                Vector3 newVelocity = currentVelocity * Time.deltaTime;
                rigidBody.velocity = currentVelocity - newVelocity;
            }
            Vector3 ahead = -transform.up;
            Vector3 rayStart = new Vector3(this.transform.position.x, this.transform.position.y + shipRayOffset, this.transform.position.z);
            Ray ray = new Ray(rayStart, ahead);
            RaycastHit hit;
            if (Input.GetKey(KeyCode.V))
            {
                rigidBody.AddForce(transform.up * Time.deltaTime * -1500);
            }
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.tag == ("ground"))
                {
                    _anim.SetBool("departure", false);
                    _anim.SetBool("landing", true);
                    rigidBody.useGravity = true;
                    canControl = false;
                    landed = true;
                }
            }
            Debug.DrawRay(rayStart, -transform.up, Color.green);
        if (!canControl) {
            ParticleSystem.EmissionModule emissionModule = starDust.emission;
            emissionModule.rateOverDistanceMultiplier = 0f;
            }
        }
	void ClampInput(){
		RollInput = Mathf.Clamp (RollInput, -1, 1);
		PitchInput = Mathf.Clamp (PitchInput, -1, 1);
		YawInput = Mathf.Clamp (YawInput, -1, 1);
		ThrottleInput = Mathf.Clamp (ThrottleInput, -1, 1);
	}
	void CalculateRollAndPitchAngles(){
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		if(flatForward.sqrMagnitude > 0){
			flatForward.Normalize();
			Vector3 localFlatForward = transform.InverseTransformDirection(flatForward);
			PitchAngle = Mathf.Atan(localFlatForward.y);

			Vector3 flatRight = Vector3.Cross(Vector3.up, flatForward);
			Vector3 localFlatRight = transform.InverseTransformDirection(flatRight);
			RollAngle = Mathf.Atan(localFlatRight.y);
			//var rot = transform.rotation;
			//this.transform.rotation = rot * Quaternion.Euler(0, Time.deltaTime*RollInput* turningAmmount, 0);
            rigidBody.AddTorque(transform.up * 10* RollInput* turningAmmount);
        }
	}

	void AutoLevel(){
		BankedTurnAmount = Mathf.Sin (RollAngle);
		if(RollInput ==0){
			RollInput =-RollAngle*AutoRollLevel;
		}
		if(PitchInput ==0){
			PitchInput =-PitchAngle*AutoPitchLevel;
			PitchInput-= Mathf.Abs(BankedTurnAmount*BankedTurnAmount*AutoTurnPitch);
		}
	}
	void CalculateForwardSpeed(){
		Vector3 localVelocity = transform.InverseTransformDirection(rigidBody.velocity);
		ForwardSpeed = Mathf.Max(0, localVelocity.z);
	}
	void ControlThrottle(){
		if(Immobilized){
			ThrottleInput = -0.5f;
		}
		Throttle = Mathf.Clamp01(Throttle + ThrottleInput * Time.deltaTime * ThrottleChangeSpeed);
		EnginePower = Throttle * MaxEnginePower;
	}
	void CalculateDrag(){
		float extraDrag = rigidBody.velocity.magnitude*DragIncreaseFactor;
		rigidBody.drag = (AirBrakes ? (OriginalDrag +extraDrag)* AirBreaksEffect : OriginalDrag + extraDrag)+0.5f;
		rigidBody.angularDrag = OriginalAngularDrag * ForwardSpeed / 1000 + OriginalAngularDrag;
	}
	void CalculateLinearForces(){
		Vector3 forces = Vector3.zero;
		forces +=EnginePower* transform.forward;
		rigidBody.AddForce(forces);
	}
	void CalculateTorque(){
		Vector3 torque = Vector3.zero;
		torque +=PitchInput* PitchEffect* transform.right;
		torque += YawInput* YawEffect* transform.up;
		torque += -RollInput*transform.forward;
		torque += BankedTurnAmount* BankedTurnEffect*transform.up;

		rigidBody.AddTorque(torque * AeroFactor);
	}
	public void Immobilize(){
		Immobilized = true;
	}
	public void Reset(){
		Immobilized = false;
	}
	public void HideReflection(){
		shipReflProbes.SetActive (false);
	}
	public void ShowReflection(){
		shipReflProbes.SetActive (true);
	}
	public void HideDockedCar(){
		if (dockedCar != null) {
			dockedCar.SetActive (false);
		}
	}
	public void ShowDockedCar(){
		if (dockedCar != null) {
			dockedCar.SetActive (true);
		}
	}
//HYPERDRIVE
	private void hyperdrive(){
		Camera.main.GetComponent<MouseOrbitSimple>().cameraHyperDrive();
	}
    public void AutoLevelShip() {        
        AutoRollLevel = 1000;
        AutoPitchLevel = 5;
    }
    public void ResetAutoLevel()
    {
        AutoRollLevel = 0;
        AutoPitchLevel = 0;
    }
}
