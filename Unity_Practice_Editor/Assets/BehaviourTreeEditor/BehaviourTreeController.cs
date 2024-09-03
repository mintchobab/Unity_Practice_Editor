using UnityEngine;

public class BehaviourTreeController : MonoBehaviour
{
    public BehaviourTree BehaviourTree;

    private BehaviourTreeContext context;

    private void Awake()
    {
        context = new BehaviourTreeContext(gameObject);
    }

    private void Update()
    {
        BehaviourTree.Evaluate(context);
    }
}
