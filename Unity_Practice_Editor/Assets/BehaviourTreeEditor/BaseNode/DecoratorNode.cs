namespace Mintchobab
{
    [System.Serializable]
    public class DecoratorNode : BehaviourNode
    {
        public DecoratorNode(string guid) : base(guid) { }

        protected BehaviourNode childNode;

        public override void Init(BehaviourTree tree)
        {
            base.Init(tree);

            if (childNodes[0] == null)
            {
                UnityEngine.Debug.LogError($"{nameof(DecoratorNode)} : Child Node is Null");
                return;
            }

            childNode = childNodes[0];
        }

        public override void Refresh()
        {
            base.Refresh();

            childNode.Refresh();
        }

        public override NodeStates Evaluate()
        {
            return NodeState = NodeStates.Failure;
        }
    }
}
