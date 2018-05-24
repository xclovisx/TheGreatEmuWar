using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.ObjectModel;

using Valve.VR.InteractionSystem;

public class Triggertest : MonoBehaviour
{

    private Hand hand;                     // The hand object


    void Start()
    {
        // Get the hand componenet
        hand = GetComponent<Hand>();

    }

    void Update()
    {
        // === NULL REFERENCE === //
        if (hand.controller.GetHairTriggerDown())
        {
            Debug.Log("Trigger");
        }

    }
}
