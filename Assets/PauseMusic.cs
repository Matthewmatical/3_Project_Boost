using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMusic : MonoBehaviour 
{
    AudioSource audioSource;
	// Use this for initialization
	void Start () 
	{
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Rocket.isPaused)
        {
            audioSource.volume = .01f;
        }
        else
        { 
            audioSource.volume = .05f;
        }

        if (Rocket.isTurbo)
        {
            audioSource.volume = .03f;
        }
        else
        {
            audioSource.volume = .05f;
        }
    
    }
}
