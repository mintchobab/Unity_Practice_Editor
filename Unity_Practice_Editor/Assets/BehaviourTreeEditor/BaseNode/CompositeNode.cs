using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompositeNode : BehaviourNode
{
    public CompositeNode(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        return NodeState.Failure;
    }
}
