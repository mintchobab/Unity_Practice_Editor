namespace Mintchobab
{
    [System.Serializable]
    public class RootNode : BehaviourNode
    {
        public RootNode(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            foreach (BehaviourNode node in childNodes)
            {
                node.Evaluate();
            }

            return NodeState = NodeStates.Success;
        }
    }
}
