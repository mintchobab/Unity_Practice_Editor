using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNode2 : ActionNode
{
    public TestNode2(string guid) : base(guid) { }


    public override void Init(BehaviourTree tree)
    {
        base.Init(tree);
    }


    public override NodeState Evaluate()
    {
        string testString = tree.Blackboard.GetData<string>("Test Key");
        Debug.LogWarning("Blackboard Ãâ·Â : " + testString);

        return NodeState.Success;
    }
}
