using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//  script gebruikt bij  {[CameraRig]/Controller (right)/Gun/Canvas-health/RED/GREEN} script  bij GREEN 
public class counter : MonoBehaviour {

    public static float killCounter;
    public static float hpCounter;
    public static bool spawn;
    public Text kill;
    public Text hp;
    public float time;

    void Start () {
        spawn = true;
        killCounter = 0;
        hpCounter = 100;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hpCounter >= 0)
        {
            kill.text = "P. " + killCounter.ToString();
            hp.text =  hpCounter.ToString();
            transform.localScale = new Vector3(1, hpCounter / 100, 1);
        }
        if (hpCounter <= 0)
        {
            spawn = false;
            time += Time.deltaTime;
            if(time >= 5) { SceneLoader(2); }
            
                


        }


    }


    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);

    }

    
}   
