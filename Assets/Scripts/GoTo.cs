using UnityEngine;
using System.Collections;

public class GoTo : MonoBehaviour
{
    public Transform target;
    private int stop = 0;


    void Update()
    {
        if (stop == 0)
        {
            Vector3 relativePos = (target.position + new Vector3(0, 1.5f, 0)) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.localRotation;

            transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            transform.Translate(0, 0, 3 * Time.deltaTime);
        }
        if (stop == 1)
        {
            Vector3 relativePos = (target.position + new Vector3(0, 0, 0)) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.localRotation;

            transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            transform.Translate(3 * Time.deltaTime, 0, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI Shield")
        {
            stop += 1;
        }
    }
}
