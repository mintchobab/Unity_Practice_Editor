using UnityEngine;

[System.Serializable]
public class MovePosition : ActionNode
{
    [SerializeReference, NodeField]
    public float MoveSpeed;

    [SerializeReference]
    private float moveTime;

    [NodeProperty]
    public float MoveTime
    {
        get => moveTime;
        set => moveTime = value;
    }

    public MovePosition(string guid) : base(guid) { }


    public override NodeState Evaluate()
    {
        Debug.LogWarning("Move Speed : " + MoveSpeed);

        tree.Context.Transform.position = Vector3.Lerp(tree.Context.Transform.position, new Vector3(100, 10, 100), Time.deltaTime);
        return NodeState.Success;
    }
}
