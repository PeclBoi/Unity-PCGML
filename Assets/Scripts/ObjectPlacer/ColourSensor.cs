using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSensor : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetUnderneigthColour();
        }
    }

    public Color GetUnderneigthColour()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, -Vector3.up, out hit);
        Renderer rend = hit.transform.GetComponent<Renderer>();
        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;

        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        var colour = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        return colour;
    }
}
