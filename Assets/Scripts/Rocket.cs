using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class Rocket : MonoBehaviour
{
    int Level;

    [SerializeField] bool godMode = true;
    [SerializeField] float rotateControl = 150f;
    [SerializeField] int thrustControl;

    bool isAlive = true;
    public static bool isTurbo;
    public Text countText;
    public int fuelGain = 100;

    public Camera main;
    public Camera fpver;
    bool isFirstPerson = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float brakeControl = 1f;
    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip levelSound;

    [SerializeField] ParticleSystem enginePart;
    [SerializeField] ParticleSystem enginePart2;
    [SerializeField] ParticleSystem enginePart3;
    [SerializeField] ParticleSystem diePart;
    [SerializeField] ParticleSystem levelPart;

    public int fuelAmount = 100;

    const float baseDrag = 0.2f;
    const int startingThrust = 10000;

    void Start ()
    {
        ToggleCameraMode(); //Set 3rd Person At Start
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

	
	void Update ()
    {
        FuelStuff();
    

        if (Debug.isDebugBuild)
        {
            debugMode(); 
        }
        if (isAlive)
        {
            ReceieveCameraInput();

            ApplyTurbo();
            AirBrake();
            Thrust();
            Rotate();
            ResetPos();
        }

	}

    private void ReceieveCameraInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            ToggleCameraMode();
        }

    }

    private void ToggleCameraMode()
    {
        if (isFirstPerson)
        {
            main.enabled = false;
            fpver.enabled = true;
            isFirstPerson = false;

        }
        else
        {
            fpver.enabled = false;
            main.enabled = true;
            isFirstPerson = true;
        }
    }

    private void FuelStuff()
    {
        if (godMode == true)
        {
            return;
        }
        else
        {
            OutofGas();
        }

    }

    private void OutofGas()
    {
        if (fuelAmount <= 0)
        {
            if (isAlive) { DeathCondition(); }

        }
        else
        {
            fuelAmount = fuelAmount - 1;
            countText.text = ("Fuel Reserve: " + fuelAmount.ToString());

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
        isTurbo = true;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            int doubleThrust = startingThrust * 4;
            thrustControl = doubleThrust;
        }
        else
        { 
            isTurbo = false;
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
            enginePart2.Stop();
            enginePart3.Stop();
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
        enginePart2.Play();
        enginePart3.Play();
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control

        float rotationThisFrame = rotateControl * Time.deltaTime;
            
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fuel":
                
                fuelAmount = fuelAmount + fuelGain;
                Destroy(other.gameObject);
                break;
            default:
                break;              
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        //Guard Collision When Not Controlling
        if (!isAlive) { return; }

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
        isAlive = false;
        diePart.Play();
        enginePart.Stop();
        enginePart2.Stop();
        enginePart3.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(dieSound);
        Invoke("ReloadScene", 3f); //change time to parameter
    }
    private void LevelTransition()
    {
        isAlive = false;
        audioSource.Stop();
        audioSource.PlayOneShot(levelSound);
        levelPart.Play();
        Invoke("LoadNextScene", 5f); //change time to parameter
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


