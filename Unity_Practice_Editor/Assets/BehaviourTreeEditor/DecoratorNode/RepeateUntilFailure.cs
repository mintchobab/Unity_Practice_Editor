using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeateUntilFailure : DecoratorNode
{
    public RepeateUntilFailure(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        while (true)
        {
            NodeState nodeState = childNode.Evaluate();

            if (nodeState == NodeState.Failure)
                break;
        }

        return nodeState;
    }
}
