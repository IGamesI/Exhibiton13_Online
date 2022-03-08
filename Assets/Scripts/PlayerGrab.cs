﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerGrab : MonoBehaviour
{
	public GameObject InputHandeler;
	public bool lookingAtObject = false;
	public GameObject grabObj;

	private GameObject oldObj;
	private bool grabing;

	private XRGrabInteractable grabInteractableComp;
	private Rigidbody rigidbodyComp;

	private void Update()
	{
		bool interact = InputHandeler.GetComponent<PlayerInputHandeler>().Interact;
		bool relesing = InputHandeler.GetComponent<PlayerInputHandeler>().Release;

		if (lookingAtObject && interact && !grabing)
		{
			grabing = true;
			MoveObjectToGrab();
		}

		if (grabing && relesing)
		{
			grabing = false;
			ReleaseObject();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Grab"))
		{
            lookingAtObject = true;
			oldObj = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Grab"))
		{
            lookingAtObject = false;
			oldObj = null;
		}
	}
	void CopyComponent(Component original, GameObject destination)
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
	}


	void MoveObjectToGrab()
	{
		grabInteractableComp = oldObj.GetComponent<XRGrabInteractable>();
		rigidbodyComp = oldObj.GetComponent<Rigidbody>();


		Destroy(oldObj.GetComponent<XRGrabInteractable>());
		Destroy(oldObj.GetComponent<Rigidbody>());
		Transform grabPosition = grabObj.transform;
		oldObj.transform.parent = grabPosition;
		oldObj.transform.position = grabPosition.position;
	}

	void ReleaseObject()
	{
		print("Hello");
		oldObj.transform.parent = null;
		CopyComponent(grabInteractableComp, oldObj);
		CopyComponent(rigidbodyComp, oldObj);
		oldObj = null;
	}
}
