using UnityEngine;

[System.Serializable]
public class MovePosition : ActionNode
{
    [SerializeReference, NodeField]
    public float MoveSpeed;

    public MovePosition(string guid) : base(guid) { }


    // 시작할 때 실행되는 함수가 필요하다 or 맨처음 한번만 실행하는...???
    public override NodeState Evaluate(BehaviourTree tree)
    {
        Debug.LogWarning("Move Speed : " + MoveSpeed);

        tree.Context.Transform.position = Vector3.Lerp(tree.Context.Transform.position, new Vector3(100, 10, 100), Time.deltaTime);
        return NodeState.Success;
    }
}
