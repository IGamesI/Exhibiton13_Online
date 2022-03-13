using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabManager : MonoBehaviour
{
	PhotonView view;
	
	private void Start()
	{
        view = GetComponent<PhotonView>();
	}
	
	void CopyComponent(Component original, GameObject destination)
	{
		if (view.IsMine)
		{
			System.Type type = original.GetType();
			Component copy = destination.AddComponent(type);
		}
	}

	[PunRPC]
	void GrabObject(string oldObjName, string grabObjName)
	{
		print("GrabObject");
		GameObject oldObj = GameObject.Find(oldObjName);
		GameObject grabObj = GameObject.Find(grabObjName);

		XRGrabInteractable grabInteractableComp = oldObj.GetComponent<XRGrabInteractable>();
		Component rigidbodyComp = oldObj.GetComponent<Rigidbody>();
		PhotonRigidbodyView rigidbodyView = oldObj.GetComponent<PhotonRigidbodyView>();

		Destroy(grabInteractableComp);
		Destroy(rigidbodyView);
		Destroy(rigidbodyComp);
		oldObj.tag = "Untagged";
	}

	[PunRPC]
	void ReleaseObject(string oldObjName)
	{
		print("ReleaseObject");
		GameObject oldObj = GameObject.Find(oldObjName);
		print(oldObj);
		oldObj.transform.parent = null;

		oldObj.AddComponent<XRGrabInteractable>();
		oldObj.AddComponent<Rigidbody>();
		oldObj.AddComponent<PhotonRigidbodyView>();
		oldObj.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
		oldObj.tag = "Grab";
	}

	[PunRPC]
	void SetGameObjectTransform(string objectName, Vector3 newPos, Quaternion newRot)
	{
		GameObject oldObj = GameObject.Find(objectName);
		oldObj.transform.position = newPos;
		oldObj.transform.rotation = newRot;
	}
}
