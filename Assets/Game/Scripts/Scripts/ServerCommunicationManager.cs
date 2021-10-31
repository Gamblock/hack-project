using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;


public class ServerCommunicationManager : MonoBehaviour
{
    public VoidEventChannelSO onServerDataNotFound;
        
    public PasswordContainerSO password;
    public CharacterInfoSO character;
    public User user = new User();
    private string emailkeyKey = "EMAIL";
    private string userIDKey = "USERID";
    private string modelIDKey = "MODELID";

    private Model model;
    private string email;
    
    public void ShowEmail(string mail)
    {
        PlayerPrefs.SetString(emailkeyKey,mail);
        StartCoroutine(GetInfo(mail));
    }
    
   
    public IEnumerator GetInfo(string emailParam)
    {
        Debug.Log("get");
        WWWForm form = new WWWForm();
       
        using (UnityWebRequest www =
            UnityWebRequest.Get("https://binance-hack.herokuapp.com/api/user/getUserByEmail/" + emailParam))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                user = JsonUtility.FromJson<User>(www.downloadHandler.text);
                PlayerPrefs.SetString(userIDKey,user._id);
            }
        }
    }
    
    public void GetAllDataFromServer(RawImage image,CharacterInfoSO characterInfoSo)
    {
        StartCoroutine(GetModelFromServer(characterInfoSo));
        StartCoroutine(GetTextureFromServer(image));
    }
    public void GetCharacterFromServer(CharacterInfoSO charachterToOverride)
    {
        StartCoroutine(GetModelFromServer(charachterToOverride));
    }
    IEnumerator GetModelFromServer(CharacterInfoSO characterToOverride)
    {
       
        string ID = "";
        using (UnityWebRequest www =
            UnityWebRequest.Get("https://binance-hack.herokuapp.com/api/model/getModelByUserId/" + PlayerPrefs.GetString(userIDKey)))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                onServerDataNotFound.RaiseEvent();
            }
            else
            {
                model = JsonUtility.FromJson<Model>(www.downloadHandler.text);
                JsonUtility.FromJsonOverwrite(model.unityGameModel, characterToOverride);
                PlayerPrefs.SetString(modelIDKey,model._id);
            }
            
        }
        
        
    }
    IEnumerator GetTextureFromServer(RawImage image)
    {
       
        string ID = "";
        using (UnityWebRequest www =
            UnityWebRequest.Get("https://binance-hack.herokuapp.com/api/model/getModelByUserId/" + PlayerPrefs.GetString(userIDKey)))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                onServerDataNotFound.RaiseEvent();
            }
            else
            {
                model = JsonUtility.FromJson<Model>(www.downloadHandler.text);
                PlayerPrefs.SetString(modelIDKey,model._id);
                UnityWebRequest image_www = UnityWebRequestTexture.GetTexture(model.base_image);
                yield return image_www.SendWebRequest();
                image.texture = DownloadHandlerTexture.GetContent(image_www);
            }
        }
    }
    
   public void MintNFT()
    {
        Application.OpenURL("https://binance-hack-frontend.herokuapp.com/tokenMint?modelId=" + PlayerPrefs.GetString(modelIDKey));
    }
  
    
    public class User
    {
        public string _id;
        
        public string firstname;
        public string lastname;
        public string username;
        public string email;
        public string password;
        public string password_repeat;
        public bool printed;
        public string created_at;
        public string updatedAt;
        public string walletAddress;
    } 
    public class Model
    {
        public string _id;
        public string name;
        public string description;
        public string unityGameModel;
        public string nft_image;
        public string base_image;
        public bool printed;
        public string token;
        public string updatedAt;
        public string createdAt;
        public string user;
    }
}