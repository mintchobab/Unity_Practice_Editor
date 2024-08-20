using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviourNode : ScriptableObject
{
    private BehaviourNode parentNode;
    private List<BehaviourNode> childrenNode = new List<BehaviourNode>();

    private NodeState state;

    public string guid;

    public Vector2 position;


    public BehaviourNode(BehaviourTree tree, string guid, Vector2 position)
    {
        parentNode = null;
    }


    public void SetParentNode(BehaviourNode node)
    {
        this.parentNode = node;
    }


    public virtual NodeState Evaluate()
    {
        return NodeState.Failure;
    }


    public static BehaviourNode CreateBehaviourNode(BehaviourTree tree, string guid, Vector2 position)
    {
        BehaviourNode node = ScriptableObject.CreateInstance<BehaviourNode>();

        node.name = "New Node";
        node.guid = guid;
        node.position = position;

        tree.AddNode(node);
        tree.SetRoot(node);

        AssetDatabase.AddObjectToAsset(node, tree);
        AssetDatabase.SaveAssets();

        return node;
    }
}
