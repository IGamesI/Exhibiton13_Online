using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
	public Vector3 newPos;
	public Quaternion newRot;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerMovment>().UpdatePosition(newPos, newRot);
		}
	}
}
