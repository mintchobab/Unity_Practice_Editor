using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestActionNode2 : ActionNode
{
    public TestActionNode2(string guid) : base(guid) { }

    public override NodeState Evaluate(BehaviourTree tree)
    {
        Debug.LogWarning("�׽�Ʈ2222");

        return NodeState.Failure;
    }
}
