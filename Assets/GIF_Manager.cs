using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIF_Manager : MonoBehaviour
{
    //frames
    [SerializeField]
    Texture[] Frames;

    // speed
    [SerializeField]
    float framesPerSecond = 10f;

    void Update()
    {
        // get index of frame
        int index = (int)(Time.time * framesPerSecond) % Frames.Length;
        // check if the Texture array don't equal null
        if (Frames[index] != null)
        {
            // get Renderer of this gameobject
            this.gameObject.GetComponent<Renderer>().material.mainTexture = Frames[index];
        }
    }
}