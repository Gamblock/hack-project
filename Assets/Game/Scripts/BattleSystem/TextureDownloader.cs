using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TextureDownloader : MonoBehaviour
{
   public RawImage image;

   public void GetImage()
   {
      StartCoroutine(downloadImage());
   }
   IEnumerator downloadImage()
   {
      UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://binance-hack.herokuapp.com/modelImages/9a0db2bc-6ca4-4207-a637-164a1203c816.png");
      yield return www.SendWebRequest();
      image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
   }
}
