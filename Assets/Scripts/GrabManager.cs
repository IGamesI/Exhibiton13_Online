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

		Destroy(oldObj.GetComponent<PhotonRigidbodyView>());
		Destroy(oldObj.GetComponent<XRGrabInteractable>());
		Destroy(oldObj.GetComponent<Rigidbody>());
		Transform grabPosition = grabObj.transform;
		oldObj.tag = "Untagged";
	}

	[PunRPC]
	void ReleaseObject(string oldObjName, XRGrabInteractable grabInteractableComp, Rigidbody rigidbodyComp, PhotonRigidbodyView rigidbodyView)
	{
		print("ReleaseObject");
		GameObject oldObj = GameObject.Find(oldObjName);
		print(oldObj);
		oldObj.transform.parent = null;
		CopyComponent(grabInteractableComp, oldObj);
		CopyComponent(rigidbodyComp, oldObj);
		CopyComponent(rigidbodyView, oldObj);
		oldObj.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
		oldObj.tag = "Grab";
		view.RPC("ReleaseObject", RpcTarget.AllBuffered, oldObj.name);
		oldObj = null;
	}

	[PunRPC]
	void SetGameObjectTransform(string objectName, Vector3 newPos, Quaternion newRot)
	{
		GameObject oldObj = GameObject.Find(objectName);
		oldObj.transform.position = newPos;
		oldObj.transform.rotation = newRot;
	}
}
