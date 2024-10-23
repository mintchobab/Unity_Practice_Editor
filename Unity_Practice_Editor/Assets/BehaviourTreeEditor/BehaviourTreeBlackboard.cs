using System.Collections.Generic;
using UnityEngine;

namespace Mintchobab
{
    public class BehaviourTreeBlackboard
    {
        private Dictionary<string, string> stringDic = new Dictionary<string, string>();
        private Dictionary<string, int> intDic = new Dictionary<string, int>();
        private Dictionary<string, float> floatDic = new Dictionary<string, float>();

        private Dictionary<string, GameObject> gameObjectDic = new Dictionary<string, GameObject>();

        private Dictionary<string, Component> componentDic = new Dictionary<string, Component>();


        public void Clear()
        {
            stringDic.Clear();
            intDic.Clear();
            floatDic.Clear();

            componentDic.Clear();
        }


        public void SetString(string key, string value) => SetData(stringDic, key, value);
        public void SetInt(string key, int value) => SetData(intDic, key, value);
        public void SetFloat(string key, float value) => SetData(floatDic, key, value);
        public void SetGameObject(string key, GameObject value) => SetData(gameObjectDic, key, value);
        public void SetComponent<T>(string key, T value) where T : Component => SetData(componentDic, key, value);

        public string GetString(string key) => GetData(stringDic, key, null);
        public int GetInt(string key) => GetData(intDic, key, default);
        public float GetFloat(string key) => GetData(floatDic, key, default);
        public GameObject GetGameObject(string key) => GetData(gameObjectDic, key, null);


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


        public T GetComponent<T>(string key) where T : Component
        {
            if (componentDic.TryGetValue(key, out Component component))
                return (T)component;
            else
                return null;
        }
    }
}
