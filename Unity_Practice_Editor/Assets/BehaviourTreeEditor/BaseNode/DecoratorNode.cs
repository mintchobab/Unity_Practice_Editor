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

            if (ChildNodeGuidList.Count == 1)
                childNode = tree.FindNode(ChildNodeGuidList[0]);
        }

        public override NodeState Evaluate()
        {
            return NodeState.Failure;
        }
    }
}
