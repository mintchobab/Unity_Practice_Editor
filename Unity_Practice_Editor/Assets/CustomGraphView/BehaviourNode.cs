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

        Debug.LogWarning("ºÎ¸ð ¼³Á¤ µÊ");
        Debug.LogWarning($"{guid} -> {parentNode.guid}");
    }

    public void AddChildNode(BehaviourNode node)
    {
        // TODO : ????
        if (childrenNode == null)
            childrenNode = new List<BehaviourNode>();

        if (!childrenNode.Contains(node))
        {
            childrenNode.Add(node);
        }
    }


    public virtual NodeState Evaluate()
    {
        return NodeState.Failure;
    }


    public void SetPosition(Vector2 position)
    {
        this.position = position;

        EditorUtility.SetDirty(this);
    }
}
