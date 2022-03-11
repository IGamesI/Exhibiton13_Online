using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PasswordMenu : MonoBehaviour
{
    public TMP_InputField passwordInput;
	private string password = "helli3meow8020";

    public void CheckPassword()
	{
		string enteredPassword = passwordInput.text;
		if (enteredPassword == password)
		{
			SceneManager.LoadScene("AdminPanel");
		}
	}

	public void BackButton()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
