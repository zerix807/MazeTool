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

    private bool goUp;
    private bool goDown;
    private bool turnLeft;
    private bool turnRight;
    private bool strafeLeft;
    private bool strafeRight;

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

    public void ResetAllLeft()
    {
        strafeLeft = false;
        strafeRight = false;
    }

    public void ResetAllRight()
    {
        goUp = false;
        goDown = false;
        turnLeft = false;
        turnRight = false;
    }

    public void SetUp()
    {
        goUp = true;
    }

    public void SetDown()
    {
        goDown = true;
    }

    public void SetTurnLeft()
    {
        turnLeft = true;
    }

    public void SetTurnRight()
    {
        turnRight = true;
    }

    public void SetStrafeLeft()
    {
        strafeLeft = true;
    }

    public void SetStrafeRight()
    {
        strafeRight = true;
    }

    void MovementUpDown()
    {
        //if (Input.GetKey(KeyCode.I))
        if (goUp)
        {
            upForce = 200;
        }
        //else if (Input.GetKey(KeyCode.K))
        else if (goDown)
        {
            upForce = -200;
        }
        //else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
        else if (!goUp && !goDown)
        {
            upForce = 0;
        }

        ourDrone.AddRelativeForce(Vector3.up * upForce);
    }

    void MovementStrafe()
    {
        //if (Input.GetKey(KeyCode.A))
        if (strafeLeft)
        {
            strafeForce = -200;
        }
        //else if (Input.GetKey(KeyCode.D))
        else if (strafeRight)
        {
            strafeForce = 200;
        }
        //else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        else if (!strafeLeft && !strafeRight)
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
        //if(Input.GetKey(KeyCode.J))
        if (turnLeft)
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        //if (Input.GetKey(KeyCode.L))
        if (turnRight)
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }
}
