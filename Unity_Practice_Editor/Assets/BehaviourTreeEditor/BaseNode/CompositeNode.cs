namespace Mintchobab
{
    [System.Serializable]
    public class CompositeNode : BehaviourNode
    {
        public CompositeNode(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            return NodeState = NodeStates.Failure;
        }
    }
}
