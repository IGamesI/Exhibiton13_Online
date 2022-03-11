using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
	PhotonView view;

	private void OnCollisionEnter(Collision collision)
	{
		if (view.IsMine)
		{
			if (collision.gameObject.tag == "Player")
			{
				collision.gameObject.GetComponent<PlayerMovment>().Health -= 20;
			}
		}
	}
}
