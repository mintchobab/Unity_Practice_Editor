using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonAttributeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            var buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

            if (buttonAttribute != null)
            {
                string buttonName = buttonAttribute.ButtonName ?? method.Name;

                if (GUILayout.Button(buttonName))
                {
                    method.Invoke(target, null);
                }
            }
        }
    }
}