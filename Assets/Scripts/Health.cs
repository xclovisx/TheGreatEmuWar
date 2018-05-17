using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
  
    public Text countText;
    private int count;

    private void Start()
    {
        count = 100;
        countText.text = count.ToString();
        
    }

    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor")
        {
            count--;
            
        }
        countText.text = count.ToString();

    }
 }

