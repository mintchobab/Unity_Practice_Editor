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

        public override NodeState Evaluate()
        {
            anyRunning = false;

            successCount = 0;
            failureCount = 0;

            foreach (string nodeGuid in childNodeGuidList)
            {
                var node = tree.FindNode(nodeGuid);

                if (node == null)
                {
                    Debug.LogError($"{nameof(SelectorNode)} : Child Node Not Found");
                    continue;
                }

                if (node.Evaluate() == NodeState.Success)
                {
                    successCount++;
                }
                else if (node.Evaluate() == NodeState.Failure)
                {
                    failureCount++;
                }
                else if (node.Evaluate() == NodeState.Running)
                {
                    anyRunning = true;
                }
            }

            if (successCount >= successThreshold)
                return NodeState.Success;

            if (failureCount > childNodeGuidList.Count - successThreshold)
                return NodeState.Failure;

            return anyRunning ? NodeState.Running : NodeState.Failure;
        }
    }
}
