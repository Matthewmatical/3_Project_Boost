using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class STARTGAME : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.anyKeyDown)
        {
            int currentLevel = (SceneManager.GetActiveScene().buildIndex);
            int nextLevel = (currentLevel + 1);
            SceneManager.LoadScene(nextLevel);
        }
	}
}
