using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    private int zoomOutOnSpeedUp;    //Stores speed value
    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        PanOutOnTurbo();
    }

    private void PanOutOnTurbo()
    {
        if (Rocket.isTurbo)
        {
            Camera.main.fieldOfView = 30;
        }
        else
        {
            Camera.main.fieldOfView = 22;
        }

    }
}