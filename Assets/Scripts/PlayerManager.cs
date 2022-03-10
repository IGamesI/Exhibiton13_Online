using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerManager : MonoBehaviour
{
	private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerCharacteristics;

    [Header("Player Modes")]
    public GameObject VRPlayer;
    public GameObject FlatPlayer;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
		{
            VRPlayer.SetActive(true);
            VRPlayer.transform.SetParent(null);
            Destroy(gameObject);
		} else
		{
            FlatPlayer.SetActive(true);
            FlatPlayer.transform.SetParent(null);
            Destroy(gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
