using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestActionNode3 : ActionNode
{
    public TestActionNode3(string guid) : base(guid)
    {
    }

    public override NodeState Evaluate(BehaviourTree tree)
    {
        Debug.LogWarning("Å×½ºÆ®3333");

        return NodeState.Success;
    }
}
