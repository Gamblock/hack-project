using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ScreenCapturer : MonoBehaviour
{
    private Camera screenshotCamera;
    private static ScreenCapturer instance;
    private bool takeScreenShotOnNextFrame;
    public RawImage outputImage;
    private string userID = "USERID";
    public CharacterInfoSO textureHolderSo;
    public string key = "KEY";
    

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
                new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect,0,0);
            renderResult.Apply();
            outputImage.texture = renderResult;
            textureHolderSo.cardTexture = renderResult;
            byte[] byteArray = renderResult.EncodeToPNG();
            StartCoroutine(InitiateModelOnServer(byteArray)) ;
            RenderTexture.ReleaseTemporary(renderTexture);
            screenshotCamera.targetTexture = null;
        }
    }
    
    private IEnumerator InitiateModelOnServer(byte[] bytes)
    {
        Debug.Log("https://binance-hack.herokuapp.com/api/model/create/" + PlayerPrefs.GetString(userID));
        WWWForm form = new WWWForm();
        form.AddBinaryData("image",bytes);
        form.AddField("unity_password","ebcb6eb1-c67a-49ad-9550-a573f2b0d55b");
        form.AddField("name",textureHolderSo.charNAme);
        form.AddField("printed","false");
        form.AddField("token","token");
        form.AddField("description","description");
        form.AddField("unityGameModel",JsonUtility.ToJson(textureHolderSo));
        var w = UnityWebRequest.Post("https://binance-hack.herokuapp.com/api/model/create/" + PlayerPrefs.GetString(userID),form);
        yield return w.SendWebRequest();
        if (w.result != UnityWebRequest.Result.Success)
        {
           Debug.Log(w.error);
        }
        else
        {
            Debug.Log(w.downloadHandler.text);
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
