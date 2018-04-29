using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    int Level;
    enum State { Alive, Dying, Transcending}
    State state = State.Alive;
    [SerializeField] bool godMode = true;
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotateControl = 150f;
    [SerializeField] int thrustControl;


    [SerializeField] float brakeControl = 1f;
    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip levelSound;
    [SerializeField] ParticleSystem enginePart;
    [SerializeField] ParticleSystem diePart;
    [SerializeField] ParticleSystem levelPart;

    const float baseDrag = 0.2f;
    const int startingThrust = 1500;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

	
	void Update ()
    {
        if (Debug.isDebugBuild)
        {
            debugMode(); 
        }
        if (state == State.Alive)
        {

            ApplyTurbo();
            AirBrake();
            Thrust();
            Rotate();
            ResetPos();
        }

	}

    private void debugMode()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            godMode = !godMode;
        }
    }

    private void ApplyTurbo()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            int doubleThrust = startingThrust * 4;
            thrustControl = doubleThrust;
        }
        else
        {
            thrustControl = startingThrust;
        }
            
    }
    private void AirBrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            rigidBody.drag = (brakeControl);
        }
        else
        {
            rigidBody.drag = (baseDrag);  //make into variable
        }
    }
    private void Thrust()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            ApplyThrust();
        }
        else
        {
            enginePart.Stop();
            audioSource.Stop();
        }
    }
    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustControl * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSound);
        }
        enginePart.Play();
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control

        float rotationThisFrame = rotateControl * Time.deltaTime;
            
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control

    }
    private void ResetPos()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.isKinematic = true;
            rigidBody.position = new Vector3(0, 3, 0);
            rigidBody.rotation = Quaternion.identity;
            rigidBody.isKinematic = false;
        }
            
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //Guard Collision When Not Controlling
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                LevelTransition();
                break;
            default:
                if (godMode == true)
                {
                    return;
                }
                DeathCondition();
                break;

        }
    }

    private void DeathCondition()
    {
        state = State.Dying;
        diePart.Play();
        enginePart.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(dieSound);

        Invoke("ReloadScene", 3f); //change time to parameter
    }
    private void LevelTransition()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelSound);
        levelPart.Play();
        Invoke("LoadNextScene", 3f); //change time to parameter
    }

    private void ReloadScene()
    {
        int Level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(Level);
    }
    private void LoadNextScene()
    {
        int currentLevel = (SceneManager.GetActiveScene().buildIndex);
        int nextLevel = (currentLevel + 1);
        if ((nextLevel) == (SceneManager.sceneCountInBuildSettings))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
        }

    }
}


