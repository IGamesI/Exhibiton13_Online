using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public bool lookingAtObject = false;
	public GameObject grabObj;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Grab"))
		{
            lookingAtObject = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Grab"))
		{
            lookingAtObject = false;
		}
	}

	void MoveObjectToGrab()
	{
		Transform grabPosition = grabObj.transform;
	}
}
