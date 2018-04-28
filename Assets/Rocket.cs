using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotateControl = 150f;
    [SerializeField] float thrustControl = 50f;
    [SerializeField] float brakeControl = 1f;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

	
	void Update ()
    {
        AirBrake();
        Thrust();
        Rotate();
        ResetPos();
	}

    private void AirBrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            rigidBody.drag = (brakeControl);
        }
        else
        {
            rigidBody.drag = (0.2f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Fuel":
                //acquire fuel
                break;
            default:
                print("Die");
                break;                

        }
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {

            rigidBody.AddRelativeForce(Vector3.up * thrustControl);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
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

}


