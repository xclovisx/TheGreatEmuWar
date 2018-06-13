using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour {
	private GameObject Player;
	public Transform PlayerCamTarget;
	private Transform target;
	public Transform camOrigin;
	private Vector3 wallHitPos;
	public float tempDist;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
	
	public float xMinLimit = -30f;
    public float xMaxLimit = 50f;
 
    public float distanceMin = 5f;
    public float distanceMax = 15f;
 
	public float x = 0.0f;
	public float y = 0.0f;
	 
	private playerControl playerScript;
 
	// Use this for initialization
	void Start () {		
		Player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		playerScript = Player.GetComponent<playerControl>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
        Vector3 angles = transform.localEulerAngles;
        x = angles.y;
        y = angles.x;

	}
 
	void Update () {
		target = PlayerCamTarget;

    if (target) {
		AvoidCollisions();
        x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
		y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			if(playerScript.shooting){//weapon kickback
				y -= playerScript.currentWeapon.weaponKickBack;
				x -= Random.Range (playerScript.currentWeapon.weaponKickBack*2.5f, -playerScript.currentWeapon.weaponKickBack*2.5f);
			}
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        Quaternion rotation = Quaternion.Euler(y, x, 0); 

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}

	void AvoidCollisions(){
		RaycastHit wallHit = new RaycastHit ();
		if (Physics.Linecast (camOrigin.transform.position, target.transform.position, out wallHit)) {
			wallHitPos = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z);		
		}
		tempDist = Vector3.Distance(wallHitPos, this.transform.position);
		if (tempDist> distanceMax){
			tempDist = distanceMax;
		}
		distance = Mathf.Lerp (distance, tempDist, Time.deltaTime * 5f);
		if (distance < 0.1f) {
			distance = 0.7f;
		}
	}
 
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}