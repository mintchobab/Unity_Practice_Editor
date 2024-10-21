using System.Collections.Generic;

namespace Mintchobab
{
    [System.Serializable]
    public class SequenceNode : CompositeNode
    {
        private List<ActionNode> childActionNodes;

        public SequenceNode(string guid) : base(guid)
        {
        }

        public override void Init(BehaviourTree tree)
        {
            base.Init(tree);

            childActionNodes = childNodes.ConvertAll(x => x as ActionNode);
        }

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

            foreach (ActionNode actionNode in childActionNodes)
            {
                actionNode.Refresh();
            }

            return NodeState = NodeStates.Success;
        }
    }
}
