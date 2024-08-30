using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovePosition : ActionNode
{
    [SerializeReference]
    public float MoveSpeed = 10f;

    public MovePosition(string guid) : base(guid) { }



    // ������ �� ����Ǵ� �Լ��� �ʿ��ϴ� or ��ó�� �ѹ��� �����ϴ�...???
    public override NodeState Evaluate(BehaviourTree tree)
    {
        tree.Context.Transform.position = Vector3.Lerp(tree.Context.Transform.position, new Vector3(100, 10, 100), Time.deltaTime);
        return NodeState.Success;
    }
}
