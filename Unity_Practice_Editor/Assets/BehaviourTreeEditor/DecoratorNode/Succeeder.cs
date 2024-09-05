public class Succeeder : DecoratorNode
{
    public Succeeder(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        childNode.Evaluate();

        return NodeState.Success;
    }
}
