using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ScreenCapturer : MonoBehaviour
{
    private Camera screenshotCamera;
    private static ScreenCapturer instance;
    private bool takeScreenShotOnNextFrame;
    public RawImage outputImage;
    public Texture2D tex;
    public CharacterInfoSO textureHolderSo;
    

    private void Awake()
    {
        screenshotCamera = GetComponent<Camera>(); 
        instance = this;
    }

    private void OnPostRender()
    {
        if (takeScreenShotOnNextFrame)
        {
            takeScreenShotOnNextFrame = false;
            RenderTexture renderTexture = screenshotCamera.targetTexture;

            Texture2D renderResult =
                new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect,0,0);

            tex = renderResult;
           renderResult.Apply();
            outputImage.texture = renderResult;
            textureHolderSo.cardTexture = renderResult;
            
            
            byte[] byteArray = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScrenShot.png", byteArray);
            Debug.Log("screenshot");
            
            RenderTexture.ReleaseTemporary(renderTexture);
            screenshotCamera.targetTexture = null;
        }
    }

    private void TakeScreenShot(int width, int height)
    {
        screenshotCamera.targetTexture = RenderTexture.GetTemporary(width, height);
        takeScreenShotOnNextFrame = true;
    }

    public static void TakeScreenShot_Static(int width, int height)
    {
        instance.TakeScreenShot(width,height);
    }
}
