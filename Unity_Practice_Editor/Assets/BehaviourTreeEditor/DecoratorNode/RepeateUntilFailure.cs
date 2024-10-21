namespace Mintchobab
{
    public class RepeateUntilFailure : DecoratorNode
    {
        public RepeateUntilFailure(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            while (true)
            {
                NodeState = childNode.Evaluate();

                if (NodeState == NodeStates.Failure)
                    break;
            }

            return NodeState;
        }
    }
}
