using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOOM : MonoBehaviour 
{
    [SerializeField] public GameObject player;
    [SerializeField] [Range(0, 1)] float CreepSpeed = 0.05f;
	// Use this for initialization
	void Start () 
	{
        
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Rocket.isPaused)
        {

        }
        else
        { 
        transform.position = transform.position + Vector3.up * CreepSpeed;
        } 
    }
}
