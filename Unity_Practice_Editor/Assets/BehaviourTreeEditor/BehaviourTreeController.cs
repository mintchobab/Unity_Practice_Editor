using UnityEngine;

public class BehaviourTreeController : MonoBehaviour
{
    public BehaviourTree BehaviourTree;

    private void Awake()
    {
        BehaviourTreeContext context = new BehaviourTreeContext(gameObject);
        BehaviourTree.Init(context);
    }

    private void Update()
    {
        BehaviourTree.Evaluate();
    }
}
