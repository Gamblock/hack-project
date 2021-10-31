using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CaptureScreenNonStatic : MonoBehaviour
{
    private Camera screenshotCamera;
    private static ScreenCapturer instance;
    private bool takeScreenShotOnNextFrameForGame;
    private bool takeScreenShotOnNextFrameForNFT = false;
    public RawImage outputImage;
    private string userID = "USERID";
    public CharacterInfoSO textureHolderSo;
    public string key = "KEY";
    private string modelIDKey = "MODELID";
    

    private void Awake()
    {
        screenshotCamera = GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenShotOnNextFrameForNFT)
        {
            takeScreenShotOnNextFrameForNFT = false;
            RenderTexture renderTexture = screenshotCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect,0,0);
            renderResult.Apply();
            outputImage.texture = renderResult;
            textureHolderSo.cardTexture = renderResult;
            byte[] byteArray = renderResult.EncodeToPNG();
           
            StartCoroutine(InitiateNFTOnServer(byteArray)) ;
            RenderTexture.ReleaseTemporary(renderTexture);
            screenshotCamera.targetTexture = null;
        }
        
        
    }
    
    private IEnumerator InitiateNFTOnServer(byte[] bytes)
    {
        
        WWWForm form = new WWWForm();
        form.AddBinaryData("image",bytes);
        form.AddField("unity_password","ebcb6eb1-c67a-49ad-9550-a573f2b0d55b");
        var w = UnityWebRequest.Post("https://binance-hack.herokuapp.com/api/model/uploadTokenImage/" + PlayerPrefs.GetString(modelIDKey),form);
        yield return w.SendWebRequest();
        if (w.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("suck");
        }
        else
        {
            Debug.Log("byu");
        }
    }
    

    public  void TakeScreenShot(int width, int height)
    {
        screenshotCamera.targetTexture = RenderTexture.GetTemporary(width, height);
        takeScreenShotOnNextFrameForNFT = true;
    }
}


