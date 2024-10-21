using System.Collections.Generic;
using UnityEngine;

namespace Mintchobab
{
    public class BehaviourTreeBlackboard
    {
        private Dictionary<string, string> stringDic = new Dictionary<string, string>();
        private Dictionary<string, int> intDic = new Dictionary<string, int>();
        private Dictionary<string, float> floatDic = new Dictionary<string, float>();

        private Dictionary<string, Object> objectDic = new Dictionary<string, Object>();


        public void Clear()
        {
            stringDic.Clear();
            intDic.Clear();
            floatDic.Clear();

            objectDic.Clear();
        }


        public void SetString(string key, string value) => SetData(stringDic, key, value);
        public void SetInt(string key, int value) => SetData(intDic, key, value);
        public void SetFloat(string key, float value) => SetData(floatDic, key, value);
        public void SetUnityObject<T>(string key, T value) where T : Object => SetData(objectDic, key, value);

        public string GetString(string key) => GetData(stringDic, key, null);
        public int GetInt(string key) => GetData(intDic, key, default);
        public float GetFloat(string key) => GetData(floatDic, key, default);


        private void SetData<T>(Dictionary<string, T> dictionary, string key, T value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }

        private T GetData<T>(Dictionary<string, T> dictionary, string key, T defauleValue)
        {
            return dictionary.GetValueOrDefault(key, defauleValue);
        }


        public T GetUnityObject<T>(string key) where T : Object
        {
            if (objectDic.TryGetValue(key, out Object component))
                return (T)component;
            else
                return null;
        }
    }
}
