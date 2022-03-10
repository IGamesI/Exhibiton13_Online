using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

public class SpawnPlayers : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerCharacteristics;

    [Header("Player Modes")]
    public GameObject VRPlayer;
    public GameObject FlatPlayer;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 3 ,Random.Range(minY, maxY));
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            PhotonNetwork.Instantiate(VRPlayer.name, randomPosition, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(FlatPlayer.name, randomPosition, Quaternion.identity);
        }

    }

}
