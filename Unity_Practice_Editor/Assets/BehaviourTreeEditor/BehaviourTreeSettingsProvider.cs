using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeSettingsProvider : SettingsProvider
{
    private static BehaviourTreeSettings settings;

    public BehaviourTreeSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
    {
        settings = BehaviourTreeSettings.GetOrCreateSettings();
    }

    public override void OnGUI(string searchContext)
    {
        base.OnGUI(searchContext);

        GUILayout.Label("Node Texture", EditorStyles.boldLabel);

        settings.SequenceTexture = EditorGUILayout.ObjectField("Sequence Node Texture", settings.SequenceTexture, typeof(Texture2D), false) as Texture2D;
        settings.SelectorTexture = EditorGUILayout.ObjectField("Selector Node Texture", settings.SelectorTexture, typeof(Texture2D), false) as Texture2D;

        if (GUI.changed)
        {
            settings.Save();
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateBehaviourTreeSettingsProvider()
    {
        return new BehaviourTreeSettingsProvider("Project/Behaviour Tree Settings", SettingsScope.Project);
    }
}
