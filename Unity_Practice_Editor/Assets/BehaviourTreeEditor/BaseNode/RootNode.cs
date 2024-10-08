using UnityEngine;

namespace Mintchobab
{
    [System.Serializable]
    public class RootNode : BehaviourNode
    {
        public RootNode(string guid) : base(guid) { Debug.LogWarning("bbbbbb"); }

        public override NodeState Evaluate()
        {
            foreach (string nodeGuid in childNodeGuidList)
            {
                var node = tree.FindNode(nodeGuid);

                if (node == null)
                {
                    Debug.LogError($"{nameof(SelectorNode)} : Child Node Not Found");
                    continue;
                }

                // TODO : �׽�Ʈ
                node.Evaluate();
            }

            return NodeState.Success;
        }
    }
}
