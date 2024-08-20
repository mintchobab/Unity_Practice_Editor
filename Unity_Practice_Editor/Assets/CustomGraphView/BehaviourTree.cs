using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum NodeState
{
    Running,
    Success,
    Failure
}

[Serializable]
public class BehaviourTree : ScriptableObject
{
    [field: SerializeField]
    public List<BehaviourNode> NodeList { get; private set; } = new List<BehaviourNode>();

    public BehaviourNode RootNode { get; private set; }


    public void Update()
    {
        if (RootNode != null)
            RootNode.Evaluate();
    }

    public void SetRoot(BehaviourNode node)
    {
        RootNode = node;
    }

    public void AddNode(BehaviourNode node)
    {
        if (!NodeList.Contains(node))
            NodeList.Add(node);
        else
            Debug.LogError($"{nameof(BehaviourTree)} : Node is Exist");
    }

    public BehaviourNode FindNode(string guid)
    {
        BehaviourNode node = NodeList.Find(x => x.guid == guid);

        if (node == null)
            Debug.LogError($"{nameof(BehaviourTree)} : Not Find Node");

        return node;
    }
}


#if UNITY_EDITOR

public class BehaviourTreeEditor : Editor
{
    private static string SavePath = "Assets/BehaviourTree.asset";

    [MenuItem("Custom/Behaviour Tree/Create New Tree")]
    public static void CreateNewTree()
    {
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(SavePath);

        BehaviourTree newTree = ScriptableObject.CreateInstance<BehaviourTree>();
        AssetDatabase.CreateAsset(newTree, uniquePath);


        string guid = GUID.Generate().ToString();

        BehaviourNode.CreateBehaviourNode(newTree, guid, new Vector2(100f, 100f));
        AssetDatabase.SaveAssets();

        Selection.activeObject = newTree;
    }
}

#endif
