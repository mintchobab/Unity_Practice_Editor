namespace Mintchobab
{
    [System.Serializable]
    public class ActionNode : BehaviourNode
    {
        public ActionNode(string guid) : base(guid) { }

        public override NodeState Evaluate()
        {
            return NodeState.Failure;
        }
    }
}
