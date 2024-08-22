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


    public BehaviourNode FindNode(string guid)
    {
        BehaviourNode node = NodeList.Find(x => x.guid == guid);

        if (node == null)
            Debug.LogError($"{nameof(BehaviourTree)} : Not Find Node");

        return node;
    }


    // view도 생성하기
    public BehaviourNode CreateBehaviourNode(string guid, Vector2 position, bool isRoot = false)
    {
        BehaviourNode node = ScriptableObject.CreateInstance<BehaviourNode>();

        node.name = "New Node";
        node.guid = guid;
        node.position = position;

        if (!NodeList.Contains(node))
            NodeList.Add(node);
        else
            Debug.LogError($"{nameof(BehaviourTree)} : Node is Exist");

        if (isRoot)
        {
            RootNode = node;
        }


        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }


    public void DestroyBehaviourNode(BehaviourNode node)
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        Debug.LogWarning("Path : " + path);

        //AssetDatabase.DeleteAsset(path);
        //AssetDatabase.SaveAssets();
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

        newTree.CreateBehaviourNode(guid, new Vector2(100f, 100f), true);
        AssetDatabase.SaveAssets();

        Selection.activeObject = newTree;
    }
}

#endif
