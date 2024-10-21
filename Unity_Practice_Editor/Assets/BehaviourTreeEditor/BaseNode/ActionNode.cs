namespace Mintchobab
{
    [System.Serializable]
    public class ActionNode : BehaviourNode
    {
        public ActionNode(string guid) : base(guid) { }

        public override NodeStates Evaluate()
        {
            return NodeState = NodeStates.Failure;
        }
    }
}
