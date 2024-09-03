using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecoratorNode : BehaviourNode
{
    public DecoratorNode(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        return NodeState.Failure;
    }
}
