using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class ServerCommunicationManager : MonoBehaviour
{
    [HideInInspector] public string userEmail = "admin@admin.com";
    [HideInInspector] public string userPassword = "admin";
    [HideInInspector] public const string HOST = "https://binance-hack.herokuapp.com/api/login";
    public PasswordContainerSO password;
    public CharacterInfoSO character;
    public User user = new User();
    public SetCharacterFromSO setCharacter;
    public TextMeshProUGUI errorText;

    private string email;
    

    public void ShowEmail(string mail)
    {
        email = mail;
        GetCharacterFromServer(mail);
    }

  public  IEnumerator GetCharacterFromServer(string emailParam)
    {
        WWWForm form = new WWWForm();
        form.AddField("unity_password", password.secretPassword);
        using (UnityWebRequest www =
            UnityWebRequest.Post("https://binance-hack.herokuapp.com/api/admin@admin.com/" + emailParam, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                user = JsonUtility.FromJson<User>(www.downloadHandler.text);
                if (user.model_id != null && user.model_id != "")
                {
                    string json = JsonUtility.ToJson(character);
                    CharacterInfoSO infoSo = CharacterInfoSO.CreateInstance<CharacterInfoSO>();
                    JsonUtility.FromJsonOverwrite(json, infoSo);
                    setCharacter.SetSpecifiedCharacter(infoSo);
                }
                else
                {
                    errorText.text = "no user data, invoke initial page";
                    Debug.Log("no user data, invoke initial page");
                }
            }
        }
    }

    public IEnumerator SaveCharacterToServer(CharacterInfoSO characterInfo, string emailParam)
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


    public class User
    {
        public string _id;

        public string model_id;

        //public CharacterInfoSO model_id;
        public string firstname;
        public string lastname;
        public string username;
        public string email;
        public string password;
        public string password_repeat;
        public bool printed;
        public string created_at;
        public string updatedAt;
    }
}