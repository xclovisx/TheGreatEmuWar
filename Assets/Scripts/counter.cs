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
            kill.text =  killCounter.ToString();
            hp.text =  hpCounter.ToString();
           
            transform.localScale = new Vector3(1, hpCounter / 100, 1);
        }
        if (hpCounter <= 0) { spawn = false; }
       


    }   
   
}   
