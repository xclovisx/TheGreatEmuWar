using UnityEngine;
using System.Collections;

public class MouseOrbitSimple : MonoBehaviour
{


    // The PlayerCamTarget we are following
    public Transform PlayerCamTarget;
    // The distance in the x-z plane to the PlayerCamTarget
    public float distance = 10.0f;
    // the height we want the camera to be above the PlayerCamTarget
    public float height = 5.0f;
    // How much we 
    public float heightDamping = 10f;
    public float rotationDamping = 3.0f;

    void FixedUpdate()
    {
        // Early out if we don't have a PlayerCamTarget
        if (!PlayerCamTarget)
            return;
        Vector3 followpos = new Vector3(0.0f, height, -distance);
        Quaternion lookrotation = Quaternion.identity;

        lookrotation.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        Matrix4x4 m1 = Matrix4x4.TRS(PlayerCamTarget.position, PlayerCamTarget.rotation, Vector3.one);
        Matrix4x4 m2 = Matrix4x4.TRS(followpos, lookrotation, Vector3.one);
        Matrix4x4 combined = m1 * m2;

        // THE WAY TO GET POSITION AND ROTATION FROM A MATRIX4X4:
        Vector3 position = combined.GetColumn(3);

        Quaternion rotation = Quaternion.LookRotation(
        combined.GetColumn(2),
        combined.GetColumn(1)
        );


        Quaternion wantedRotation = rotation;
        Quaternion currentRotation = transform.rotation;

        Vector3 wantedPosition = position;
        Vector3 currentPosition = transform.position;

        currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, rotationDamping * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, wantedPosition, heightDamping * Time.deltaTime);

        transform.localRotation = currentRotation;
        transform.localPosition = currentPosition;


    }
    public void cameraHyperDrive()
    {
        Vector3 originalPos = transform.localPosition;
        transform.localPosition = originalPos + Random.insideUnitSphere * 0.5f;
    }
}