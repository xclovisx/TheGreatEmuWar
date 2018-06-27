using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//  script gebruikt bij  {[CameraRig]/Controller (right)/Gun/Canvas-health/RED/GREEN} script  bij GREEN 
public class counter : MonoBehaviour {

    public static float killCounter;
    public static float hpCounter;
    public static bool spawn;
    public Text kill;
    public Text hp;
    public Text hp2;
    public Text kill2;

    void Start () {
        spawn = true;
        killCounter = 0;
        hpCounter = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (hpCounter >= 0)
        {
            kill.text = "P. " + killCounter.ToString();
            hp.text = "HP " + hpCounter.ToString();
            hp2.text =  hpCounter.ToString();
            kill2.text = killCounter.ToString();
            transform.localScale = new Vector3(1, hpCounter / 100, 1);
        }
        if (hpCounter <= 0) { spawn = false; }

      
    }   
   
}   
