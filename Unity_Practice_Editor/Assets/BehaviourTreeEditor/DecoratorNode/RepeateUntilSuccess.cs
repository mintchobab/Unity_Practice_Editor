namespace Mintchobab
{
    public class RepeateUntilSuccess : DecoratorNode
    {
        public RepeateUntilSuccess(string guid) : base(guid) { }

        public override NodeState Evaluate()
        {
            while (true)
            {
                NodeState nodeState = childNode.Evaluate();

                if (nodeState == NodeState.Success)
                    break;
            }

            return nodeState;
        }
    }
}
