using UnityEngine;

public class WaitNode : ActionNode
{
    private float waitTime;

    [NodeProperty]
    public float WaitTime
    {
        get => waitTime;
        set => waitTime = value;
    }

    private float elapsedTime;


    public WaitNode(string guid) : base(guid) { }

    public override NodeState Evaluate()
    {
        if (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            return NodeState.Running;
        }
        else
        {
            elapsedTime = 0f;
            return NodeState.Success;
        }        
    }
}
