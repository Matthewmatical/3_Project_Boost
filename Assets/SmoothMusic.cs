using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmoothMusic : MonoBehaviour 
{
    static bool AudioBegin = false;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (!AudioBegin)
        {
            audioSource.Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }

    void Start () 
	{
		
	}

    void Update()
    {
        int Level = SceneManager.GetActiveScene().buildIndex;
        if (Level == 2)
        {
            audioSource.Stop();
            AudioBegin = false;
        }

    }
}
