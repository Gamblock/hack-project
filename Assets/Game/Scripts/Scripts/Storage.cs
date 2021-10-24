using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : ScriptableObject
{
    public void SaveData<T>(string key,T value)
    {
        ES3.Save(key,value);
    }
}
