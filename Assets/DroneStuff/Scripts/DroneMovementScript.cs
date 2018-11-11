using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour {

    Rigidbody ourDrone;
    private float upForce;
    private float strafeForce;
    private float movementForwardSpeed = 200.0f;
    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationYVelocity;

    void Awake ()
    {
        ourDrone = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        MovementUpDown();
        MovementStrafe();
        MovementForward();
        MovementRotation();
        ourDrone.rotation = Quaternion.Euler(
            new Vector3(ourDrone.rotation.x, currentYRotation, ourDrone.rotation.z)
            );
    }

    void MovementUpDown()
    {
        if (Input.GetKey(KeyCode.I))
        {
            upForce = 200;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            upForce = -200;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
        {
            upForce = 0;
        }

        ourDrone.AddRelativeForce(Vector3.up * upForce);
    }

    void MovementStrafe()
    {
        if (Input.GetKey(KeyCode.A))
        {
            strafeForce = -200;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            strafeForce = 200;
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            strafeForce = 0;
        }

        ourDrone.AddRelativeForce(Vector3.right * strafeForce);
    }

    void MovementForward()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            ourDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
        }
    }
    void MovementRotation()
    {
        if(Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }
}
