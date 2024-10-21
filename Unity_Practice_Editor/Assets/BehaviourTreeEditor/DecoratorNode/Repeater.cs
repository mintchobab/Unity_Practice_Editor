namespace Mintchobab
{
    public class Repeater : DecoratorNode
    {
        [NodeField]
        private int repeatCount;

        public Repeater(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            for (int i = 0; i < repeatCount; i++)
            {
                NodeState = childNode.Evaluate();

                if (NodeState == NodeStates.Failure)
                    return NodeState;
            }

            return NodeStates.Success;
        }
    }
}
