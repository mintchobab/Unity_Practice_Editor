using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestNode : ActionNode
{
    public TestNode(string guid) : base(guid) { }

    public override void Init(BehaviourTree tree)
    {
        base.Init(tree);

        tree.Blackboard.SetData<string>("Test Key", "Test String Value");
    }

    public override NodeState Evaluate()
    {
        return NodeState.Success;
    }
}
