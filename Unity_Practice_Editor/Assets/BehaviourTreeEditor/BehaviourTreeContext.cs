using UnityEngine;

public class BehaviourTreeContext
{
    public GameObject GameObject { get; private set; }
    public Transform Transform { get; private set; }
    public Animator Animator { get; private set; }

    public BehaviourTreeContext(GameObject gameObject)
    {
        GameObject = gameObject;
        Transform = gameObject.transform;
        Animator = gameObject.GetComponent<Animator>();
    }
}
