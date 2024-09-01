using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "ScriptableObjects/Behaviour Tree", order = 1)]
public class BehaviourTree : ScriptableObject
{
    [SerializeReference]
    public List<BehaviourNode> NodeList = new List<BehaviourNode>();

    public string RootNodeGuid;

    private BehaviourNode rootNode;

    public BehaviourTreeContext Context { get; private set; }


    private void OnEnable()
    {
        if (RootNodeGuid == null)
        {
            BehaviourNode node = CreateNode(typeof(RootNode));
            RootNodeGuid = node.Guid;
        }
    }


    public BehaviourNode CreateNode(Type nodeType)
    {
        string guid = System.Guid.NewGuid().ToString();

        BehaviourNode newNode = Activator.CreateInstance(nodeType, guid) as BehaviourNode;

        NodeList.Add(newNode);
        EditorUtility.SetDirty(this);

        return newNode;
    }


    public void RemoveNode(BehaviourNode node)
    {
        if (!NodeList.Contains(node))
            return;

        if (node.Guid == RootNodeGuid)
            return;

        NodeList.Remove(node);

        EditorUtility.SetDirty(this);
    }


    public BehaviourNode FindNode(string guid)
    {
        return NodeList.Find(x => x.Guid == guid);
    }


    public void Evaluate(BehaviourTreeContext context)
    {
        if (rootNode == null)
            rootNode = FindNode(RootNodeGuid);

        if (this.Context != context)
            this.Context = context;

        rootNode.Evaluate(this);
    }
}

