using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFollow : MonoBehaviour 
{
    public GameObject player;       //Public variable to store a reference to the player game object
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    void Start () 
	{
        transform.position = new Vector3(0, 9.16f, 5);
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void Update () 
	{
        transform.position = player.transform.position + offset;
    }
}
