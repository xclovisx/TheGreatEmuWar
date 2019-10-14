using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    private GameObject Play;
    private GameObject Dropdown;

   void Awake()
    {
        Play = GameObject.FindWithTag("Play");
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Play")
        {
          Play.SetActive(false);
            SceneLoader(1);
        }
    }
    
    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
