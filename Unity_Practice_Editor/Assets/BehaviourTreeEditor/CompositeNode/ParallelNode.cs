using UnityEngine;

namespace Mintchobab
{
    public class ParallelNode : CompositeNode
    {
        [SerializeReference]
        private int successThreshold;

        [NodeProperty]
        public int SuccessThreshold
        {
            get => successThreshold;
            set => successThreshold = value;
        }

        private bool anyRunning;

        private int successCount;
        private int failureCount;


        public ParallelNode(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            anyRunning = false;

            successCount = 0;
            failureCount = 0;

            foreach (BehaviourNode node in childNodes)
            {
                if (node.Evaluate() == NodeStates.Success)
                {
                    successCount++;
                }
                else if (node.Evaluate() == NodeStates.Failure)
                {
                    failureCount++;
                }
                else if (node.Evaluate() == NodeStates.Running)
                {
                    anyRunning = true;
                }
            }

            if (successCount >= successThreshold)
                return NodeState = NodeStates.Success;

            if (failureCount > childNodeGuidList.Count - successThreshold)
                return NodeState = NodeStates.Failure;

            return anyRunning ? NodeState = NodeStates.Running : NodeState = NodeStates.Failure;
        }
    }
}
