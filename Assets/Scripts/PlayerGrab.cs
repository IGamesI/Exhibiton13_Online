using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class PlayerGrab : MonoBehaviour
{
    PhotonView view;

    public GameObject InputHandeler;
    public bool lookingAtObject = false;
    public GameObject grabObj;

    private GameObject oldObj;
    private bool grabing;

    private XRGrabInteractable grabInteractableComp;
    private Rigidbody rigidbodyComp;

    private bool hasGun = false;

    private void Start()
    {
        view = gameObject.gameObject.transform.parent.GetComponent<PhotonView>();
    }

	private void Update()
    {
        if (view.IsMine)
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

            if (hasGun && Input.GetMouseButtonDown(0))
            {
                oldObj.GetComponent<Gun>().Fire();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (view.IsMine)
        {
            if (other.CompareTag("Grab"))
            {
                lookingAtObject = true;
                oldObj = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (view.IsMine)
        {
            if (other.CompareTag("Grab"))
            {
                lookingAtObject = false;
                oldObj = null;
            }
        }
    }
    void CopyComponent(Component original, GameObject destination)
    {
        if (view.IsMine)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
        }
    }


    void MoveObjectToGrab()
    {
        if (view.IsMine)
        {
            grabInteractableComp = oldObj.GetComponent<XRGrabInteractable>();
            rigidbodyComp = oldObj.GetComponent<Rigidbody>();


            Destroy(oldObj.GetComponent<XRGrabInteractable>());
            Destroy(oldObj.GetComponent<Rigidbody>());
            Transform grabPosition = grabObj.transform;
            oldObj.transform.parent = grabPosition;
            if (oldObj.GetComponent<Gun>())
            {
                oldObj.transform.localPosition = new Vector3(0, -1.17f, 0);
                oldObj.transform.localRotation = Quaternion.Euler(90, -180, 90);
                hasGun = true;
            } else
			{
                oldObj.transform.position = grabPosition.position;
            }
        }
    }

    void ReleaseObject()
    {
        if (view.IsMine)
        {
            Transform grabPosition = grabObj.transform;
            if (hasGun)
            {
                hasGun = false;
            }

            if (oldObj == null)
            {
                oldObj = grabPosition.GetChild(0).gameObject;
            }
            oldObj.transform.parent = null;
            CopyComponent(grabInteractableComp, oldObj);
            CopyComponent(rigidbodyComp, oldObj);
            oldObj.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
            oldObj = null;
        }
    }
}
