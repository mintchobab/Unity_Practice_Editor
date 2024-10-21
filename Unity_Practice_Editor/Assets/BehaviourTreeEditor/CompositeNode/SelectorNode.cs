namespace Mintchobab
{
    [System.Serializable]
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(string guid) : base(guid)
        {
        }

        public override NodeStates Evaluate()
        {
            foreach (BehaviourNode node in childNodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.Failure:
                        continue;

                    case NodeStates.Success:
                        return NodeState = NodeStates.Success;

                    case NodeStates.Running:
                        return NodeState = NodeStates.Running;
                }
            }

            return NodeState = NodeStates.Failure;
        }
    }
}
