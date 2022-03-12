using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviour
{
    public float speed = 40;
    public GameObject bullet;
    public Transform barrel;
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    public void Fire()
    {
        GameObject spawnedBullet = PhotonNetwork.Instantiate(bullet.name, barrel.position, barrel.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = speed * barrel.forward;
        // audioSource.PlayOneShot(audioClip);
        Destroy(spawnedBullet, 2);
    }
}
