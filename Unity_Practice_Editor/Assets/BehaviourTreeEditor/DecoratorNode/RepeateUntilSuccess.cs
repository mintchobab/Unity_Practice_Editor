namespace Mintchobab
{
    public class RepeateUntilSuccess : DecoratorNode
    {
        public RepeateUntilSuccess(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            while (true)
            {
                NodeState = childNode.Evaluate();

                if (NodeState == NodeStates.Success)
                    break;
            }

            return NodeState;
        }
    }
}
