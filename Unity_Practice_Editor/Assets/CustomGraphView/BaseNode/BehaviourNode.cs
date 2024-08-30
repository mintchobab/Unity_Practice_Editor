using System;
using System.Collections.Generic;
using UnityEngine;


// 직렬화를 위한 데이터 관리 클래스
[Serializable]
public class BehaviourNodeData
{
    public string TypeName;

    public string Guid;
    public string ParentNodeGuid;
    public List<string> ChildNodeGuidList;
    public float PosX;
    public float PosY;

    // Action Node
    public float MoveSpeed;
}

public enum NodeState
{
    Running,
    Success,
    Failure
}


[System.Serializable]
public class BehaviourNode
{
    [SerializeReference]
    protected string guid;
    public string Guid
    {
        get => guid;
        set => guid = value;
    }


    [SerializeReference]
    protected string parentNodeGuid;
    public string ParentNodeGuid
    {
        get => parentNodeGuid;
        set => parentNodeGuid = value;
    }


    [SerializeReference]
    protected List<string> childNodeGuidList = new List<string>();
    public List<string> ChildNodeGuidList
    {
        get => childNodeGuidList;
        set => childNodeGuidList = value;
    }


    [SerializeReference]
    protected float posX;
    public float PosX
    {
        get => posX;
        set => posX = value;
    }


    [SerializeReference]
    protected float posY;
    public float PosY
    {
        get => posY;
        set => posY = value;
    }


    public BehaviourNode(string guid)
    {
        this.guid = guid;
    }


    protected NodeState nodeState;


    public void AddChildNode(string guid)
    {
        if (childNodeGuidList.Contains(guid))
            return;

        childNodeGuidList.Add(guid);
    }


    public void RemoveChildNode(string guid)
    {
        if (!childNodeGuidList.Contains(guid))
            return;

        childNodeGuidList.Remove(guid);
    }


    public virtual NodeState Evaluate(BehaviourTree tree)
    {
        return NodeState.Failure;
    }
}
