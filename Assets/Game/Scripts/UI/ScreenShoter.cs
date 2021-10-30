using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShoter : MonoBehaviour
{
    public RawImage img;
    public Vector2 widthHeight;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
        }
            
    }


    public void CaptureSpecificRect()
    {
        ScreenCapturer.TakeScreenShot_Static((int)widthHeight.x,(int)widthHeight.y);
    }
    public void Capture()
    {
        ScreenCapturer.TakeScreenShot_Static(400,540);
    }

    public void ReadTexture()
    {
        string base64 = PlayerPrefs.GetString("KEY");
        byte[] bytes = System.Convert.FromBase64String(base64);
        Texture2D tex = new Texture2D(400, 540);
        tex.LoadImage(bytes);
        img.texture = tex;
    }
}
