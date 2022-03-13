using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthScript : MonoBehaviour
{
	public GameObject player;

	private void Start()
	{
		player.GetComponent<PlayerMovment>().UpdateHealthRpcThing(player, player.GetComponent<PlayerMovment>().Health);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.tag == "Bullet")
		{
			print("Ouch");
			player.GetComponent<PlayerMovment>().Health -= 20;
			player.GetComponent<PlayerMovment>().UpdateHealthRpcThing(player, player.GetComponent<PlayerMovment>().Health);
			Destroy(collision.collider.gameObject);
		}
	}
}
