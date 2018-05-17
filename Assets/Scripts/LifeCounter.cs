using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{

    public Text countText;
    public Text LoseText;

    public int count;
    public bool spawn;
    
    void Start()
    {
        spawn = true;
        count = 5;
        SetCountText();
        LoseText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor" && count >= 1)
        {
            count = count - 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Lives: " + count.ToString();
        if (count <= 0)
        {
            LoseText.text = "You Lose!";
        }
    }

    private void Update()
    {
        if (count == 0)
        {
           // spawn = false;
        }
    }
}
