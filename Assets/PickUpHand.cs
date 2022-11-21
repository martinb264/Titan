using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PickUpHand : MonoBehaviour
{
    public float distToPickup = 0.3f;
    bool handClosed = false;
    public LayerMask pickuplayer;
    public SteamVR_Input_Sources handsource = SteamVR_Input_Sources.LeftHand;

    Rigidbody holdingTarget;

    private void FixedUpdate()
    {
        if(SteamVR_Actions.default_GrabGrip.GetState(handsource))
        {
            handClosed = true;
        }
        else
        {
            handClosed = false;
        }

        if(!handClosed)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, distToPickup, pickuplayer);
            if (colliders.Length > 0)
            {
                holdingTarget = colliders[0].transform.root.GetComponent<Rigidbody>();
            }
            else
            {
                holdingTarget = null;
            }

        }
        else
        {
            if(holdingTarget)
            {
                holdingTarget.velocity = (transform.position - holdingTarget.transform.position) / Time.fixedDeltaTime;

                holdingTarget.maxAngularVelocity = 20;
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(holdingTarget.transform.rotation);
                Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0,deltaRot.eulerAngles.x), Mathf.DeltaAngle(0,deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
                eulerRot *= 0.95f;
                eulerRot *= Mathf.Deg2Rad;
                holdingTarget.angularVelocity = eulerRot / Time.fixedDeltaTime;
            }
        }
    }
}
