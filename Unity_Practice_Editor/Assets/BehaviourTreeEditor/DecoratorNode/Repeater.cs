namespace Mintchobab
{
    public class Repeater : DecoratorNode
    {
        [NodeField]
        private int repeatCount;

        public Repeater(string guid) : base(guid) { }

        public override NodeState Evaluate()
        {
            for (int i = 0; i < repeatCount; i++)
            {
                NodeState nodeState = childNode.Evaluate();

                if (nodeState == NodeState.Running)
                {
                    continue;
                }
                else if (nodeState == NodeState.Failure)
                {
                    return NodeState.Failure;
                }
            }

            return NodeState.Success;
        }
    }
}
