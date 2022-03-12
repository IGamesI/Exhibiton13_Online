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
    Transform grabPosition;

    private GameObject oldObj;
    private GameObject oldObj2;
    private bool grabing;

    private XRGrabInteractable grabInteractableComp;
    private Rigidbody rigidbodyComp;
    private PhotonRigidbodyView rigidbodyView;

    private bool hasGun = false;
    public GameObject player;

    private void Start()
    {
        view = player.GetComponent<PhotonView>();
        grabPosition = grabObj.transform;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            bool interact = InputHandeler.GetComponent<PlayerInputHandeler>().Interact;
            bool relesing = InputHandeler.GetComponent<PlayerInputHandeler>().Release;

            if (lookingAtObject && interact && !grabing)
            {
                MoveObjectToGrab();
            }

            if (grabing && !hasGun)
			{
                view.RPC("SetGameObjectTransform", RpcTarget.Others, oldObj.name, oldObj.transform.position, oldObj.transform.rotation);
                oldObj.transform.position = grabPosition.position;
            } else if (grabing && hasGun)
			{
                view.RPC("SetGameObjectTransform", RpcTarget.Others, oldObj.name, oldObj.transform.position, oldObj.transform.rotation);
                oldObj.transform.position = grabPosition.position - new Vector3(0, 0.114644f, 0);
                oldObj.transform.rotation = transform.rotation *Quaternion.Euler(90, -180, 90);
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
        {
            if (other.CompareTag("Grab"))
            {
                lookingAtObject = true;
                if (!grabing)
				{
                    oldObj = other.gameObject;
                    oldObj2 = other.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (view.IsMine)
        {
            if (other.CompareTag("Grab"))
            {
                if (!grabing) { 
                    lookingAtObject = false;
                    oldObj = null;
                }
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
            grabing = true;
            grabInteractableComp = oldObj.GetComponent<XRGrabInteractable>();
            rigidbodyComp = oldObj.GetComponent<Rigidbody>();
            rigidbodyView = oldObj.GetComponent<PhotonRigidbodyView>();

            Destroy(oldObj.GetComponent<PhotonRigidbodyView>());
            Destroy(oldObj.GetComponent<XRGrabInteractable>());
            Destroy(oldObj.GetComponent<Rigidbody>());
            Transform grabPosition = grabObj.transform;
            oldObj.tag = "Untagged";
            if (oldObj.GetComponent<Gun>())
            {
                oldObj.transform.localPosition += new Vector3 (0, -1.17f, 0);
                oldObj.transform.localRotation = Quaternion.Euler(90, -180, 90);
                hasGun = true;
            } else
            {
                oldObj.transform.position = grabPosition.position;
            }

            print("Calling grab");
            view.RPC("GrabObject", RpcTarget.OthersBuffered, oldObj.name, grabObj.name);
        }
    }

    void ReleaseObject()
    {
        if (view.IsMine)
        {
            if (hasGun)
            {
                hasGun = false;
            }

            if (oldObj == null)
            {
                oldObj = oldObj2;
            }
            oldObj.transform.parent = null;
            CopyComponent(grabInteractableComp, oldObj);
            CopyComponent(rigidbodyComp, oldObj);
            CopyComponent(rigidbodyView, oldObj);
            oldObj.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
            oldObj.tag = "Grab";
            view.RPC("ReleaseObject", RpcTarget.OthersBuffered, oldObj.name, grabInteractableComp, rigidbodyComp, rigidbodyView);
            oldObj = null;
        }
    }
}
