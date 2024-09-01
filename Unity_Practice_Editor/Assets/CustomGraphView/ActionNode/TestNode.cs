using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestNode : ActionNode
{
    public TestNode(string guid) : base(guid) { }

    public override NodeState Evaluate(BehaviourTree tree)
    {
        Debug.LogWarning("Node : TestNode");

        return NodeState.Success;
    }
}
