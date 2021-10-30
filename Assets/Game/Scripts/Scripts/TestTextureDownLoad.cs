using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTextureDownLoad : MonoBehaviour
{
  public RawImage image;
  public ServerCommunicationManager serverManager;
  public Texture2D texture;
  public CharacterInfoSO character;

  public void SetTexture()
  {
    serverManager.GetAllDataFromServer(image,character);
  }
}
