using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovment : MonoBehaviour
{
    PhotonView view;

    private Vector2 Move;
    public Vector2 Look;
    public GameObject InputHandeler;

    public GameObject Camera;

    public float mouseSpeed = 100f;
    
    public float xRotation = 0f;
    


    //Move Vars
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -9.81f;

    Vector3 velocity;
    private float fallingSpeed;
    public LayerMask groundLayer;

    public Animation walkAnimation;
    public TMP_Text playerName;

    public float Health = 100;
    public bool invisible = false;
    public bool movable = true;
    
    void Update()
    {
        if (view.IsMine)
        {
            //Input
            Move = InputHandeler.GetComponent<PlayerInputHandeler>().Move;
            Look = InputHandeler.GetComponent<PlayerInputHandeler>().Look;

            if (Move.x != 0 || Move.y != 0)
            {
                if (!walkAnimation.isPlaying)
                {
                    walkAnimation.Play();
                }
            }
            else
            {
                if (walkAnimation.isPlaying)
                {
                    walkAnimation.Rewind();
                }
            }

            #region Look
            //Look
            Look = Look * mouseSpeed * Time.deltaTime;

            xRotation -= Look.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);


            Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * Look.x);
            #endregion

            #region Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump();
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Escape))
			{
                print("Exit Game");
                Application.Quit();
            }

            if (Health <= 0)
            {
                SceneManager.LoadScene("MainMenu");
            }

            if (Input.GetKeyDown(KeyCode.F5) && Input.GetKeyDown(KeyCode.Alpha6) && Input.GetKeyDown(KeyCode.Alpha3))
			{
                invisible = true;
			}
            if (invisible)
			{
                gameObject.transform.position = new Vector3(-0f, -1000f, -0f);
            }
        }
    }
    

    private void FixedUpdate()
	{
        if (view.IsMine)
        {
            if (movable)
            {
                bool isGrounded = CheckIfGrounded();
                if (isGrounded)
                {
                    fallingSpeed = 0;
                }
                else
                {
                    fallingSpeed += gravity * Time.fixedDeltaTime;
                }
                fallingSpeed += gravity * Time.fixedDeltaTime;
                controller.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

                #region Move
                Quaternion headYaw = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                Vector3 direction = headYaw * new Vector3(Move.x, 0, Move.y);

                controller.Move(direction * Time.fixedDeltaTime * speed);
                #endregion
            }
        }
    }

	void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            controller.enableOverlapRecovery = false;
        } else
		{
            //Destroy(Camera);
            Camera.GetComponent<Camera>().enabled = false;
        }

        view.RPC("UpdatePlayerName", RpcTarget.AllBufferedViaServer);
    }

    bool CheckIfGrounded()
    {
        if (view.IsMine)
        {
            // tells us if on ground
            Vector3 rayStart = transform.TransformPoint(controller.center);
            float rayLength = controller.center.y + 0.01f;
            bool hasHit = Physics.SphereCast(rayStart, controller.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
            return hasHit;
        } else
		{
            return true;
		}
    }
    void jump()
    {
        if (view.IsMine)
        {
            bool hit_bi_moola = CheckIfGrounded();
            if (hit_bi_moola)
            {
                gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 10f, ForceMode.Impulse);
            }
        }
    }

    [PunRPC]
    void UpdatePlayerName()
    {
        if (gameObject.tag == "Player")
		{
            playerName.text = view.Owner.NickName;
        }
    }

    public void UpdateHealthRpcThing(GameObject player, float health)
	{
        view.RPC("UpdateHealth", RpcTarget.OthersBuffered, player.name, health);
    }

    [PunRPC]
    void UpdateHealth(string desiredPlayerName, float updatedHealth)
    {
        GameObject desiredPlayer = GameObject.Find(desiredPlayerName);
        desiredPlayer.GetComponent<PlayerMovment>().Health = updatedHealth;
    }

    public void UpdatePosition(Vector3 pos, Quaternion rot)
	{
        movable = false;
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rot;
        StartCoroutine(waitUntilMove());
    }

    IEnumerator waitUntilMove()
	{
        yield return new WaitForSeconds(0.4f);
        movable = true;
	}
}
