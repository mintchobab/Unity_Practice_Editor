using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SequenceNode : CompositeNode
{
    public SequenceNode(string guid) : base(guid)
    {
    }

    public override NodeState Evaluate(BehaviourTree tree)
    {
        foreach (string nodeGuid in childNodeGuidList)
        {
            var node = tree.FindNode(nodeGuid);

            if (node == null)
            {
                Debug.LogError($"{nameof(SelectorNode)} : Child Node Not Found");
                continue;
            }

            switch (node.Evaluate(tree))
            {
                case NodeState.Failure:
                    nodeState = NodeState.Failure;
                    return nodeState;

                case NodeState.Success:
                    continue;

                case NodeState.Running:
                    nodeState = NodeState.Running;
                    return nodeState;

                default:
                    nodeState = NodeState.Success;
                    return nodeState;
            }
        }

        nodeState = NodeState.Success;
        return nodeState;
    }
}
