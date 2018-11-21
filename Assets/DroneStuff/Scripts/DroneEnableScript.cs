using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnableScript : MonoBehaviour {

    public GameObject myDrone;
    private bool isActive = false;

    public void SwitchDrone()
    {
        isActive = !isActive;
        myDrone.SetActive(isActive);
    }
}
