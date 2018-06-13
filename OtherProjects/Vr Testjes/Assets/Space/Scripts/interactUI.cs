using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class interactUI : MonoBehaviour {
	private Camera playerCam;
	public Text interactText;
	public AudioClip pickSound;
	private Animator _anim;
	public Canvas _canv;
	private AudioSource auSource;
	private bool canUse = true;
	// Use this for initialization
	void Start () {
		playerCam = Camera.main;
		_anim = this.GetComponent<Animator> ();
		_canv = this.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float dist = Vector3.Distance(playerCam.transform.position, this.transform.position);
		if(dist<6){
			interactText.enabled = true;
			if (Input.GetKeyDown(KeyCode.E)&&canUse){
				auSource = this.gameObject.transform.parent.GetComponent<AudioSource>();
				auSource.clip = pickSound;
				auSource.loop = false;
				canUse = false;
				auSource.Play();
				StartCoroutine(DestroySelf());
			}
		}
		else{
			interactText.enabled = false;
		}
		_anim.SetFloat ("distToPlayer", dist);
		_canv.transform.LookAt(playerCam.transform);
	}
	IEnumerator DestroySelf(){
		yield return new WaitForSeconds (0.7f);
		Destroy(this.transform.parent.gameObject);
	}
}
