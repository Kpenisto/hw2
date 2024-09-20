/*
Author: Kyle Peniston
Date: 9/19/2024
Description: RotatorScript.cs controls the rotation of pickup/powerup game objects. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

}
