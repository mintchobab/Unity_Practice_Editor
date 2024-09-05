namespace Mintchobab
{
    [System.Serializable]
    public class CompositeNode : BehaviourNode
    {
        public CompositeNode(string guid) : base(guid) { }

        public override NodeState Evaluate()
        {
            return NodeState.Failure;
        }
    }
}
