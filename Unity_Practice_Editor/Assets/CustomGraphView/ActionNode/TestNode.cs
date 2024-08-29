using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestNode : ActionNode
{
    public TestNode(string guid) : base(guid) { }

    public override NodeState Evaluate(BehaviourTree tree)
    {
        if (tree == null)
            Debug.LogWarning(11111111111111);

        if (tree.Context == null)
            Debug.LogWarning(2222222222222);

        if (tree.Context.GameObject == null)
            Debug.LogWarning(3333333333333);

        Debug.LogWarning("Name : " + tree.Context.GameObject.name);
        
        return NodeState.Success;
    }
}
