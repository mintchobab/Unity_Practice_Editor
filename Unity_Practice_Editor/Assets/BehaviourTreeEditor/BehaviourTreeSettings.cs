using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mintchobab
{
#if UNITY_EDITOR
    public class BehaviourTreeSettings : ScriptableObject
    {
        public const string SettingsPath = "Assets/BehaviourTreeEditor/BehaviourTreeSettings.asset";

        public Texture2D SequenceTexture;
        public Texture2D SelectorTexture;
        public Texture2D WaitTexture;


        private static BehaviourTreeSettings instance;

        public static BehaviourTreeSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetOrCreateSettings();
                }

                return instance;
            }
        }

        public static BehaviourTreeSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<BehaviourTreeSettings>(SettingsPath);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<BehaviourTreeSettings>();
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
