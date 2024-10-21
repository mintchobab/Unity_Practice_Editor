using UnityEngine;

namespace Mintchobab
{
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


        public override void Init(BehaviourTree tree)
        {
            base.Init(tree);
            elapsedTime = 0f;
        }


        public override void Refresh()
        {
            base.Refresh();
            elapsedTime = 0f;
        }


        public override NodeStates Evaluate()
        {
            if (elapsedTime < waitTime)
            {
                elapsedTime += Time.deltaTime;
                return NodeStates.Running;
            }
            else
            {
                return NodeStates.Success;
            }
        }
    }
}
