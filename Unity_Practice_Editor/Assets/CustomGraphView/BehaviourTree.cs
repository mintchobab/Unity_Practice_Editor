using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;

[CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "ScriptableObjects/Behaviour Tree", order = 1)]
public class BehaviourTree : ScriptableObject
{
    //[HideInInspector]
    public BehaviourNode RootNode;

    //[HideInInspector]
    public List<BehaviourNode> NodeList = new List<BehaviourNode>();


    private void OnEnable()
    {
        if (RootNode == null)
        {
            string guid = GUID.Generate().ToString();

            RootNode = new BehaviourNode(guid);
            NodeList.Add(RootNode);
        }
    }

    //public BehaviourNode CreateNode()
    //{
    //    string guid = GUID.Generate().ToString();
    //    BehaviourNode newNode = new BehaviourNode(guid);

    //    NodeList.Add(newNode);
    //    EditorUtility.SetDirty(this);

    //    return newNode;
    //}






    // TODO : 합치는 방향에 대해서 생각해보기
    public ActionNode CreateActionNode()
    {
        string guid = GUID.Generate().ToString();
        ActionNode newNode = new ActionNode(guid);

        NodeList.Add(newNode);
        EditorUtility.SetDirty(this);

        return newNode;
    }


    // TODO : 합치는 방향에 대해서 생각해보기
    public SelectorNode CreateSelectorNode()
    {
        string guid = GUID.Generate().ToString();
        SelectorNode newNode = new SelectorNode(guid);

        NodeList.Add(newNode);
        EditorUtility.SetDirty(this);

        return newNode;
    }

    // TODO : 합치는 방향에 대해서 생각해보기
    public SequenceNode CreateSequenceNode()
    {
        string guid = GUID.Generate().ToString();
        SequenceNode newNode = new SequenceNode(guid);

        NodeList.Add(newNode);
        EditorUtility.SetDirty(this);

        return newNode;
    }












    public void RemoveNode(BehaviourNode node)
    {
        if (!NodeList.Contains(node))
            return;

        if (node.Guid == RootNode.Guid)
            return;

        NodeList.Remove(node);

        EditorUtility.SetDirty(this);
    }


    public BehaviourNode FindNode(string guid)
    {
        return NodeList.Find(x => x.Guid == guid);
    }
}

[System.Serializable]
public class BehaviourNode
{
    //[HideInInspector]
    public string Guid;

    //[HideInInspector]
    public string ParentNodeGuid;

    //[HideInInspector]
    public List<string> ChildNodeList = new List<string>();

    [HideInInspector]
    public float PosX;

    [HideInInspector]
    public float PosY;


    // 이거 없어져야함
    public BehaviourNode(string guid)
    {
        this.Guid = guid;
    }


    public void SetPosition(Vector2 position)
    {
        PosX = position.x;
        PosY = position.y;
    }


    public void RemoveParnetNode()
    {
        ParentNodeGuid = null;
    }


    public void SetParentNode(string guid)
    {
        ParentNodeGuid = guid;
    }

    public void AddChildNode(string guid)
    {
        if (ChildNodeList.Contains(guid))
            return;

        ChildNodeList.Add(guid);
    }


    public void RemoveChildNode(string guid)
    {
        if (!ChildNodeList.Contains(guid))
            return;

        ChildNodeList.Remove(guid);
    }
}




public class ActionNode : BehaviourNode
{
    public ActionNode(string guid) : base(guid) { Debug.LogWarning("Action Node 생성"); }

}

public class SelectorNode : BehaviourNode
{
    public SelectorNode(string guid) : base(guid) { Debug.LogWarning("Selector Node 생성"); }


}

public class SequenceNode : BehaviourNode
{
    public SequenceNode(string guid) : base(guid) { Debug.LogWarning("Sequence Node 생성"); }


}

