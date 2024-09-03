using UnityEngine;

[System.Serializable]
public class SelectorNode : CompositeNode
{
    public SelectorNode(string guid) : base(guid)
    {
    }

    public override NodeState Evaluate()
    {
        foreach (string nodeGuid in childNodeGuidList)
        {
            var node = tree.FindNode(nodeGuid);

            if (node == null)
            {
                Debug.LogError($"{nameof(SelectorNode)} : Child Node Not Found");
                continue;
            }

            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    continue;

                case NodeState.Success:
                    nodeState = NodeState.Success;
                    return nodeState;

                case NodeState.Running:
                    nodeState = NodeState.Running;
                    return nodeState;

                default:
                    continue;
            }
        }

        nodeState = NodeState.Failure;
        return nodeState;
    }
}
