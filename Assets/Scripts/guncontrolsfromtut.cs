using UnityEngine;
using System.Collections;
//  script gebruikt bij  {[CameraRig]/Controller (left)} script  bij Controller (left)
//  script gebruikt bij  {[CameraRig]/Controller (right)} script  bij Controller (right)

public class guncontrolsfromtut : MonoBehaviour
{
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    public float fireRate;
    private float nextFire;

    public GameObject Bullet_Emitter;

    public GameObject Bullet;

    public float Bullet_Up_Force;

    //////////////////////////////////
    public AudioClip shootSound;

    private AudioSource source;
    /////////////////////////////////

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (controller.GetPressDown(triggerButton) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //Debug.Log("Pressed");
            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position + new Vector3(0.0f, 0.0f, 0.0f), Bullet_Emitter.transform.rotation) as GameObject;

            Temporary_Bullet_Handler.transform.Rotate(Vector3.forward * 90);
            Temporary_Bullet_Handler.transform.Rotate(Vector3.right * -90);
            Temporary_Bullet_Handler.transform.Rotate(Vector3.up * -90);

            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

            source.PlayOneShot(shootSound, 1);

            //Temporary_RigidBody.AddForce(transform.forward * Bullet_Up_Force);

            Destroy(Temporary_Bullet_Handler, 5.0f);

        }
    }
}