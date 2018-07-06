using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backtomenu : MonoBehaviour {
    public float time;
	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
       
        time += Time.deltaTime;
        if (time >= 10) { SceneLoader(0); }




    }





public void SceneLoader(int SceneIndex)
{
    SceneManager.LoadScene(SceneIndex);

}


}
