using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Diagnostics;

public class TakeImage : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera linkedCamera;

    Texture2D texture2D;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Print))
        {
            RenderTexture.active = linkedCamera.targetTexture;

            var tex = new Texture2D(linkedCamera.targetTexture.width, linkedCamera.targetTexture.height);
            tex.ReadPixels(new Rect(0, 0, linkedCamera.targetTexture.width, linkedCamera.targetTexture.height), 0, 0);
            tex.Apply();


            File.WriteAllBytes("./Assets/Images/Test.png", tex.EncodeToPNG());
            Debug.Log("Image Taken.");
        }
    }
}
