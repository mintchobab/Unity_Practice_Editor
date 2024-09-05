public class InverterNode : DecoratorNode
{
    public InverterNode(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        NodeState childState = childNode.Evaluate();

        if (childState == NodeState.Success)
            return NodeState.Failure;
        else if (childState == NodeState.Failure)
            return NodeState.Success;

        return NodeState.Running;
    }
}
