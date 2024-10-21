namespace Mintchobab
{
    [System.Serializable]
    public class ConditionNode : BehaviourNode
    {
        public ConditionNode(string inGuid) : base(inGuid) { }

        public override NodeStates Evaluate()
        {
            return NodeState = NodeStates.Failure;
        }
    }
}
