﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  
    public Text countText;
    private int count;
    public bool spawn;

    void Start()
    {
        spawn = true;
        count = 100;
        countText.text = count.ToString();
        
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor")
        {
            if (count>0) {count+=10;}
            
            
        }
        countText.text = count.ToString();

    }

    private void Update()
    {
        if (count == 0)
        {
             spawn = false;
        }
    }
}

