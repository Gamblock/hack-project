using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ServerCommunicationManager : MonoBehaviour
{
    [HideInInspector] public string userEmail = "admin@admin.com";
    [HideInInspector] public string userPassword = "admin";
    [HideInInspector] public const string HOST = "https://binance-hack.herokuapp.com/api/login";
    public VoidEventChannelSO onServerDataNotFound;
        
    public PasswordContainerSO password;
    public CharacterInfoSO character;
    public User user = new User();
    private string emailkeyKey = "EMAIL";
    private string userIDKey = "USERID";
    private string modelIDKey = "MODELID";

    private Model model;
    private string email;

    private void Awake()
    {
        ShowEmail("admin@admin.com");
        
    }

    public void SaveCharacterToServerInvoke(CharacterInfoSO characterParams)
    {
        StartCoroutine(SaveCharacterToServer(characterParams));
    }
    public void ShowEmail(string mail)
    {
        PlayerPrefs.SetString(emailkeyKey,mail);
        StartCoroutine(GetInfo(mail));

    }
    
    public IEnumerator SaveModelToServer(CharacterInfoSO characterInfo, string emailParam)
    {
        WWWForm form = new WWWForm();
        form.AddField("model_id", JsonUtility.ToJson(character));
        form.AddField("email", emailParam);
        form.AddField("unity_password", password.secretPassword);
        Debug.Log("https://binance-hack.herokuapp.com/api/updateUser/" + user._id);
        using (UnityWebRequest www =
            UnityWebRequest.Post("https://binance-hack.herokuapp.com/api/updateUser/" + user._id, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
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
    public IEnumerator SaveCharacterToServer(CharacterInfoSO characterInfo)
    {
        WWWForm form = new WWWForm();
        form.AddField("model_id", JsonUtility.ToJson(character));
        form.AddField("email", PlayerPrefs.GetString(email));
        form.AddField("unity_password", password.secretPassword);
        Debug.Log("https://binance-hack.herokuapp.com/api/updateUser/" + user._id);
        using (UnityWebRequest www =
            UnityWebRequest.Post("https://binance-hack.herokuapp.com/api/updateUser/" + user._id, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public void GetAllDataFromServer(RawImage image,CharacterInfoSO characterInfoSo)
    {
        Debug.Log("Getting data");
        StartCoroutine(GetModelFromServer(characterInfoSo));
        StartCoroutine(GetCardTextureByID(image));
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
    IEnumerator GetCardTextureByID(RawImage image)
    {
        Debug.Log("https://binance-hack.herokuapp.com/modelImages/" + PlayerPrefs.GetString(modelIDKey) + ".png");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://binance-hack.herokuapp.com/modelImages/" + PlayerPrefs.GetString(modelIDKey) + ".png");
            
        {      
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                onServerDataNotFound.RaiseEvent();
            }
            else
            {
                image.texture = DownloadHandlerTexture.GetContent(www);
            }
        }
        
    }

   
    
    public IEnumerator GetModelIDFromServerAndMint()
    {
        using (UnityWebRequest www =
            UnityWebRequest.Get("https://binance-hack.herokuapp.com/api/model/getModelIdByUserId/" + user._id))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Application.OpenURL("https://binance-hack-frontend.herokuapp.com/tokenMint?modelId=" + www.downloadHandler.text);
            }
        }
    }

    public void MintNFT()
    {
        StartCoroutine(GetModelIDFromServerAndMint());
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
        public string image;
        public bool printed;
        public string token;
        public string updatedAt;
        public string createdAt;
        public string user;
    }
}