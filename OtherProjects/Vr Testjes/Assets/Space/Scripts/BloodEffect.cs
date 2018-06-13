using UnityEngine;
using System.Collections;

public class BloodEffect : MonoBehaviour {
	private playerControl playerScript;
	private GameObject Player;
	public GameObject bloodScreen;
	public Material bloodSplat;

	public float waitTimeReset = 6f;
	public float waitTime = 5;

	// Use this for initialization
	void Start () {
		Player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		playerScript = Player.GetComponent<playerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerScript.underFire){
			if (playerScript.health < 80) {
				bloodScreen.SetActive (true);
			}
			if (waitTime < waitTimeReset){//COUNTING DOWN FROM WAITTIME TO 0 WHEN WAITTIME BECOME LESSER THAN WAITTIMERESET
			waitTime -= Time.deltaTime;
		}
		if (waitTime < 0){ 
			waitTime = 0; //don't make wait time lesser than 0
		}
			if (waitTime == 0) {//when wait time become zero
				waitTime = 5;
				playerScript.underFire = false;//we're not under fire anymore
			}
		}
		if(!playerScript.underFire && playerScript.health<100){
			playerScript.health+= Time.deltaTime*10; //REGENERATING HEALTH
		}
		if(playerScript.health>=100){//if we're hit enable blood effect
			bloodScreen.SetActive(false);
		}
		float alphaValue = 1- playerScript.health/100;
		bloodSplat.color = new Color(1.0f, 0.0f, 0.0f, alphaValue);//change  blood effect alpha 
	}
}
