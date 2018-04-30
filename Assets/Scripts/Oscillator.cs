using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(0f, 10f, 0f);
    [SerializeField] float period = 2f;

    [Range(0, 1)] [SerializeField] float movementFactor; //Tells us when at max distance
    Vector3 startPos;

	void Start ()
    {
        startPos = transform.position;
	}
	

	void Update ()
    {

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f; //approx 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offSet = movementFactor * movementVector;
        transform.position = startPos + offSet;
	}
}
