/*
Author: Kyle Peniston
Date: 9/19/2024
Description: CameraController.cs manages the camera's position relative to the player.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    [SerializeField] public float rotationSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        h = h / rotationSpeed;

        transform.position = player.transform.position;
        transform.Rotate(new Vector3(0f, 1f, 0f) * h, Space.World);
    }
}
