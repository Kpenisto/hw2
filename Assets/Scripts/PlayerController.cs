/*
Author: Kyle Peniston
Date: 9/11/2024
Description: PlayerController.cs manages the behavior of the player character in the game. 
It handles player movement, game timer, pick-up and power-up interactions, and the win/lose conditions. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Globals
    private Rigidbody rb;
    public Button restartButton;
    private bool hasWon = false;
    private bool hasLost = false;

    //Player/Camera Vars
    [SerializeField] public float speed;
    [SerializeField] public float size;
    public Camera camera;

    //Timer/Text Vars
    public Text countText;
    private int count;
    public Text winText;
    public Text powerUpText;
    float timer = 60.0f;
    private int totalPickups;

    //PowerUp Vars
    float powerUpTimer = 0.0f;
    bool powerUp = false;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Set Defaults
        count = 0;
        countText.text = "Count: " + count.ToString();
        totalPickups = GameObject.FindGameObjectsWithTag("Pickup").Length;

        winText.text = "";
        powerUpText.text = "";

        originalScale = transform.localScale;
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasWon)
        {
            timer -= Time.deltaTime;
            //Doesn't alow timer doesn't go negative
            timer = Mathf.Clamp(timer, 0, float.MaxValue);
            int seconds = Mathf.CeilToInt(timer);
            countText.text = "Timer: " + seconds.ToString() + "s";

            //Times up Game Over
            if (seconds == 0)
            {
                winText.text = "Game Over!";
                restartButton.gameObject.SetActive(true);
                hasLost = true;
            }
         }

        //PowerUp pickup timer and scaling
        if (powerUp)
        {
            powerUpTimer -= Time.deltaTime;
            powerUpTimer = Mathf.Clamp(powerUpTimer, 0, float.MaxValue);
            int powerUpSeconds = Mathf.CeilToInt(powerUpTimer);
            powerUpText.text = "PowerUp: " + powerUpSeconds.ToString() + "s";

            if (powerUpTimer <= 0)
            {
                powerUp = false;
                powerUpText.text = "";

                //Reset size when powerup ends
                ChangePlayerSizeScale(originalScale);
            }
        }
    }

    //FixedUpdate is in sync with physics engine
    void FixedUpdate()
    {
        //Movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Apply camera direction
        rb.AddForce(camera.transform.forward * v * speed);

        if (v == 0)
        {
            //Stop movement when no vertical input
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Track Pickup gameObjects
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;

            //Win condition
            if (count >= totalPickups && !hasLost)
            {
                HandleWin();
            }
        }

        //PowerUp condition
        if (other.gameObject.CompareTag("PowerUp") && !powerUp)
        {
            other.gameObject.SetActive(false);
            powerUp = true;
            powerUpTimer = 10.0f;

            //Allow powerup for 10 seconds
            powerUpText.text = "PowerUp: " + powerUpTimer.ToString() + "s";

            //Scaling
            ChangePlayerSizeScale(transform.localScale *= 5);
        }
    }

    private void HandleWin()
    {
        //Set the win flag
        hasWon = true;
        winText.text = "You Win!";
        restartButton.gameObject.SetActive(true);
    }

    private void ChangePlayerSizeScale(Vector3 _size)
    {
        //Dynamic Scaling
        transform.localScale = _size;
    }

    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
