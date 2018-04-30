using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    //Starting Parameters //////////////////////////
    public const int fuelCanister = 100;
    int currentFuel = 500;
    float startingDrag = 0.2f;
    const int startingThrust = 10000;
    public static int startingLives = 3;

    //Tweakable Params //////////////////////////
    [SerializeField] int thrustAmount;
    [SerializeField] float rotateControl = 400f;
    [SerializeField] float brakeControl = 10f;
    [SerializeField] float transitionTime = 4f;

    //Fuck Booleans //////////////////////////
    public static bool isTurbo = false;
    public static bool isPaused = false;
    public static bool isBraking = false;
    bool isFirstPerson = false;
    bool allowedControl = true;
    bool isGodMode = false;
    bool GameOver = false;

    public Text fuelText;
    public Text livesText;

    public Camera main;
    public Camera fpver;

    //Child References //////////////////////////
    Rigidbody rigidBody;
    AudioSource audioSource;

    //Component References //////////////////////////
    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip stallSound;
    [SerializeField] AudioClip levelSound;
    [SerializeField] AudioClip fuelSound;
    [SerializeField] AudioClip pauseSound;
    [SerializeField] AudioClip unpauseSound;

    [SerializeField] ParticleSystem enginePart;
    [SerializeField] ParticleSystem enginePart2;
    [SerializeField] ParticleSystem enginePart3;
    [SerializeField] ParticleSystem diePart;
    [SerializeField] ParticleSystem levelPart;


    void Start ()
    {
        ToggleCameraMode(); //Set 3rd Person At Start
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

	
	void Update ()
    {
        FuelCheck();
        LifeCounter();
        PauseInput();

        if (allowedControl)
        {
            ReceieveCameraInput();
            TurboInput();
            AirBrakeInput();
            ThrustInput();
            RotationInput();
        }

        if (Debug.isDebugBuild)
        {
            DebugCommands();
        }
    }


    // START INPUT COMMAND CODE //////////////////////////
    private void LifeCounter()
    {
        livesText.text = ("Lives: " + startingLives);
    }
    private void FuelCheck()
    {
        fuelText.text = ("Fuel Reserve: " + currentFuel.ToString());

        if (currentFuel <= -1)
        {
            if (isGodMode)
            {
            }
            else
            {
                currentFuel = 0;
                allowedControl = false;
                SlowDeath();
            }
        }
    }

    private void PauseInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            Pause();
        }
    }
    private void Pause()
    {
        if (isPaused)
        {
            allowedControl = false;
            audioSource.Stop();
            audioSource.PlayOneShot(pauseSound);
            Time.timeScale = 0;
        }
        else
        {
            audioSource.Stop();
            audioSource.PlayOneShot(unpauseSound);
            allowedControl = true;
            Time.timeScale = 1;
            audioSource.PlayOneShot(unpauseSound);
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

        }
        else
        {
            fpver.enabled = false;
            main.enabled = true;
        }
    }

    private void DebugCommands()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isGodMode = !isGodMode;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.isKinematic = true;
            rigidBody.position = new Vector3(0, 16, 0);
            rigidBody.rotation = Quaternion.identity;
            rigidBody.isKinematic = false;
        }
    }

    private void TurboInput()
    {
        ApplyTurbo();
    }
    private void ApplyTurbo()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isTurbo = true;
            thrustAmount = startingThrust * 2; // TODO ADD VAR FOR ITEM UPGRADE HERE BOOST 2X, 3X, 4X
        }
        else
        {
            isTurbo = false;
            thrustAmount = startingThrust;
        }
    }

    private void AirBrakeInput()
    {
        ApplyAirBrake();
    }
    private void ApplyAirBrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.drag = (brakeControl);
            isBraking = true;
        }
        else
        {
            rigidBody.drag = (startingDrag);
            isBraking = false;
        }
    }

    private void ThrustInput()
    {

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
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
    } //LOOK INTO THIS. DOUBLE FUEL USAGE IF TURBO, FIX SOUND BUG 
    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustAmount * Time.deltaTime);
        currentFuel = currentFuel - 5; //Fuel Drain
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSound);
        }
        enginePart.Play();
        enginePart2.Play();
        enginePart3.Play();
    }

    private void RotationInput()
    {
        rigidBody.angularVelocity = Vector3.zero; //disable physics when turning
        float rotationThisFrame = rotateControl * Time.deltaTime; //makes sure turn rate matches framerate
        ApplyRotation(rotationThisFrame);
    }
    private void ApplyRotation(float rotationThisFrame)
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            currentFuel = currentFuel - 1; //Fuel Drain
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            currentFuel = currentFuel - 1; //Fuel Drain
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }



    // START COLLISION DETECTION CODE //////////////////////////
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fuel":
                audioSource.Stop();
                audioSource.PlayOneShot(fuelSound);
                currentFuel = currentFuel + fuelCanister;
                Destroy(other.gameObject);
                break;
            default:
                break;              
        }

    } //PICK UP WORLD ITEMS HERE

    void OnCollisionEnter(Collision collision)
    {

        if (!allowedControl) { return; } //Stops additional collision after loss of control from death state or something

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                LevelTransition();
                break;

            default:
                if (isGodMode == true)
                {
                    return;
                }
                ExplosiveDeath();
                break;

        }
    }

    private void ExplosiveDeath()
    {
        OutOfLives();
        allowedControl = false;
        diePart.Play();
        enginePart.Stop();
        enginePart2.Stop();
        enginePart3.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(dieSound);
        Invoke("ReloadScene", transitionTime);
    }
    private void SlowDeath()
    {
        OutOfLives();
        allowedControl = false;
        enginePart.Stop();
        enginePart2.Stop();
        enginePart3.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(stallSound);
        Invoke("ReloadScene", transitionTime); 
    }

    private void OutOfLives()
    {
        if (startingLives > 0)
        {
            startingLives = startingLives - 1;
        }
        else
        {
            GameOver = true;
            print("DISPLAY GAME OVER SCREEN");
            startingLives = 3;
        }
    }

    private void LevelTransition()  // this is very messy ugly code
    {
        allowedControl = false;
        audioSource.Stop();
        audioSource.PlayOneShot(levelSound);
        levelPart.Play();
        Invoke("LoadNextScene", 5f); //change time to parameter
    }

    private void ReloadScene()
    {
        int currentLevel = (SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(currentLevel);
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


