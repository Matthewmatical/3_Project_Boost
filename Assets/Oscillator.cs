using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] [SerializeField] float movementFactor; //Tells us when at max distance
    Vector3 startPos;

	void Start ()
    {
        startPos = transform.position;
	}
	

	void Update ()
    {
        Vector3 offSet;
        transform.position = startPos + offSet;
	}
}
