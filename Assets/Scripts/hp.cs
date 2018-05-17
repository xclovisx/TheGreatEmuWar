using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp : MonoBehaviour
{
    
    public Text countText;
   

    private void Start()
    {
       
  
        
        transform.localScale = new Vector3(1, float.Parse(countText.text) / 100, 1);

    }
    void Update()
    {

        transform.localScale = new Vector3(1, float.Parse(countText.text) / 100, 1);
    }

}

