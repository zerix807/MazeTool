using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelEnable : MonoBehaviour {

	public GameObject myPanel;
	private bool isActive = false;

	public void SwitchPanel() {
		isActive = !isActive;
		myPanel.SetActive (isActive);
	}
}
