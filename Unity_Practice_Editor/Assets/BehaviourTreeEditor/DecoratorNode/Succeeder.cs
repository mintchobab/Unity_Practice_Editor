namespace Mintchobab
{
    public class Succeeder : DecoratorNode
    {
        public Succeeder(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            childNode.Evaluate();

            return NodeState = NodeStates.Success;
        }
    }
}
