using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionNode : BehaviourNode
{
    public ActionNode(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        return NodeState.Failure;
    }
}
