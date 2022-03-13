using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField nameInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
        print("Creating room");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
        print("Joining room");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void AdminPanel()
	{
        SceneManager.LoadScene("PasswordMenu");
	}

    public void HelpMenu()
	{
        SceneManager.LoadScene("Help");
	}

    public void ExitGame()
	{
        print("Exit Game");
        Application.Quit();
    }
}
