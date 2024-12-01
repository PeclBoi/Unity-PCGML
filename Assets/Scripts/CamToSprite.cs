using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class CamToSprite : MonoBehaviour
{
    // Start is called before the first frame update
    //get reference of camera from inspector
    public Camera camera;
    public int height = 515;
    public int width = 516;
    int depth = 24;

    //method to render from camera
    public Sprite CaptureScreen()
    {
        RenderTexture renderTexture = new RenderTexture(width, height, depth);
        Rect rect = new Rect(0, 0, width, height);
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        camera.targetTexture = renderTexture;
        camera.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        camera.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        Destroy(renderTexture);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        return sprite;
    }

    public void SimpleCaptureTransparentScreenshot(string screengrabfile_path)
    {
        // Depending on your render pipeline, this may not work.
        var bak_cam_targetTexture = camera.targetTexture;
        var bak_cam_clearFlags = camera.clearFlags;
        var bak_RenderTexture_active = RenderTexture.active;

        var tex_transparent = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Must use 24-bit depth buffer to be able to fill background.
        var render_texture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
        var grab_area = new Rect(0, 0, width, height);

        RenderTexture.active = render_texture;
        camera.targetTexture = render_texture;
        camera.clearFlags = CameraClearFlags.SolidColor;

        // Simple: use a clear background
        camera.backgroundColor = Color.clear;
        camera.Render();
        tex_transparent.ReadPixels(grab_area, 0, 0);
        tex_transparent.Apply();

        // Encode the resulting output texture to a byte array then write to the file
        byte[] pngShot = ImageConversion.EncodeToPNG(tex_transparent);
        File.WriteAllBytes(screengrabfile_path, pngShot);

        camera.clearFlags = bak_cam_clearFlags;
        camera.targetTexture = bak_cam_targetTexture;
        RenderTexture.active = bak_RenderTexture_active;
        RenderTexture.ReleaseTemporary(render_texture);

        Texture2D.Destroy(tex_transparent);

    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            SimpleCaptureTransparentScreenshot("./Assets/Images/Background.png");
        }
    }

}
