using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float w = 1f;
    public int t = 0;
    public Text countText;
    private int count;

    private void Start()
    {
        transform.localScale = new Vector3(1, w, 1);
        count = 100;
        countText.text = "Count:" + count.ToString();
    }
    void Update()
    {
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Meteor")
        {
            if (w > 0 && t == 0)
            {

                w -= 1f / 100;
                count--;
            }
            else
            {
                t = 1;
                w += 1f / 100;
                count++;
                if (w == 1f)
                {
                    t = 0;
                }


            }

            countText.text = "Count:" + count.ToString();
            transform.localScale = new Vector3(1, w, 1);

        }
    }
}
