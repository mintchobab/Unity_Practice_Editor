namespace Mintchobab
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            NodeStates childState = childNode.Evaluate();

            if (childState == NodeStates.Success)
                return NodeState = NodeStates.Failure;
            else if (childState == NodeStates.Failure)
                return NodeState = NodeStates.Success;

            return NodeState = NodeStates.Running;
        }
    }
}
