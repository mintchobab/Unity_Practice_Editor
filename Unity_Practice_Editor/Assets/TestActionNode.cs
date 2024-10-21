using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mintchobab
{
    public class TestActionNode : ActionNode
    {
        public TestActionNode(string guid) : base(guid) { }

        private float elapsedTime = 0f;
        private float maxTime = 3f;

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
            if (elapsedTime < maxTime)
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