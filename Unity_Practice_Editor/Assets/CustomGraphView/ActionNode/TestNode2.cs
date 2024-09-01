using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNode2 : ActionNode
{
    public TestNode2(string guid) : base(guid) { }

    public override NodeState Evaluate(BehaviourTree tree)
    {
        Debug.LogWarning("Node : TestNode2");

        return NodeState.Success;
    }
}
