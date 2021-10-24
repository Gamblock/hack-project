using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : ScriptableObject
{
    public void GetDataByKey(string key)
    {
        ES3.Load(key);
    }
    public void SaveData<T>(string key,T value)
    {
        ES3.Save(key,value);
    }
}
