using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "ScriptableObjects/Behaviour Tree", order = 1)]
public class BehaviourTree : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeReference]
    public List<BehaviourNode> NodeList = new List<BehaviourNode>();
    //public List<BehaviourNodeData> NodeDataList = new List<BehaviourNodeData>();

    public string RootNodeGuid;

    private BehaviourNode rootNode;
    public BehaviourTreeContext Context { get; private set; }


    public void OnBeforeSerialize()
    {
        //NodeDataList.Clear();

        //for (int i = 0; i < NodeList.Count; i++)
        //{
        //    BehaviourNodeData nodeData = new BehaviourNodeData();
        //    nodeData.TypeName = NodeList[i].GetType().Name;
        //    nodeData.Guid = NodeList[i].Guid;
        //    nodeData.ParentNodeGuid = NodeList[i].ParentNodeGuid;
        //    nodeData.ChildNodeGuidList = NodeList[i].ChildNodeGuidList;
        //    nodeData.PosX = NodeList[i].PosX;
        //    nodeData.PosY = NodeList[i].PosY;

        //    if (NodeList[i] is MovePosition movePosition)
        //    {
        //        nodeData.MoveSpeed = movePosition.MoveSpeed;
        //    }

        //    NodeDataList.Add(nodeData);
        //}
    }

    // 직렬화 후
    public void OnAfterDeserialize()
    {
        //NodeList.Clear();

        //for (int i = 0; i < NodeDataList.Count; i++)
        //{
        //    Type type = Type.GetType(NodeDataList[i].TypeName);

        //    BehaviourNode node = Activator.CreateInstance(type, NodeDataList[i].Guid) as BehaviourNode;
        //    node.PosX = NodeDataList[i].PosX;
        //    node.PosY = NodeDataList[i].PosY;
        //    node.ParentNodeGuid = NodeDataList[i].ParentNodeGuid;
        //    node.ChildNodeGuidList = NodeDataList[i].ChildNodeGuidList;

        //    if (node is MovePosition movePosition)
        //    {
        //        movePosition.MoveSpeed = NodeDataList[i].MoveSpeed;
        //    }

        //    NodeList.Add(node);
        //};
    }


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

