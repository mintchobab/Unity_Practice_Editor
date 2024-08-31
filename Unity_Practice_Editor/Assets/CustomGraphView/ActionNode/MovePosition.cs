using UnityEngine;

[System.Serializable]
public class MovePosition : ActionNode
{
    [SerializeReference, NodeField]
    public float MoveSpeed;

    public MovePosition(string guid) : base(guid) { }


    // ������ �� ����Ǵ� �Լ��� �ʿ��ϴ� or ��ó�� �ѹ��� �����ϴ�...???
    public override NodeState Evaluate(BehaviourTree tree)
    {
        Debug.LogWarning("Move Speed : " + MoveSpeed);

        tree.Context.Transform.position = Vector3.Lerp(tree.Context.Transform.position, new Vector3(100, 10, 100), Time.deltaTime);
        return NodeState.Success;
    }
}
