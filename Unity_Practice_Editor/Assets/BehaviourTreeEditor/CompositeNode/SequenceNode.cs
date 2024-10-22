namespace Mintchobab
{
    [System.Serializable]
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            foreach (BehaviourNode node in childNodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.Running:
                        return NodeState = NodeStates.Running;

                    case NodeStates.Failure:
                        return NodeState = NodeStates.Failure;
                }
            }

            foreach (BehaviourNode node in childNodes)
            {
                if (node is CompositeNode)
                    continue;

                node.Refresh();
            }

            return NodeState = NodeStates.Success;
        }
    }
}
