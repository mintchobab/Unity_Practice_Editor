using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard : MonoBehaviour
{
    private Dictionary<Type, IDictionary> blackboardDictionary = new Dictionary<Type, IDictionary>();


    public void SetData<T>(string keyName, T value)
    {
        if (!blackboardDictionary.ContainsKey(typeof(T)))
            blackboardDictionary.Add(typeof(T), new Dictionary<string, T>());

        IDictionary dic = blackboardDictionary[typeof(T)];

        if (dic.Contains(keyName))
        {
            dic[keyName] = value;
        }
        else
        {
            dic.Add(keyName, value);
        }
    }


    public T GetData<T>(string keyName)
    {
        if (!blackboardDictionary.ContainsKey(typeof(T)))
            return default(T);

        IDictionary dic = blackboardDictionary[typeof(T)];

        if (!dic.Contains(keyName))
            return default(T);

        return (T)dic[keyName];
    }
}
