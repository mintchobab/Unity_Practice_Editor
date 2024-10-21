using UnityEngine;

namespace Mintchobab
{
    public class BehaviourTreeController : MonoBehaviour
    {
        public BehaviourTree BehaviourTree;

        private void Awake()
        {
            AddBlackboardDatas();

            BehaviourTree.Init();
        }

        private void Update()
        {
            BehaviourTree.Evaluate();
        }

        private void AddBlackboardDatas()
        {
            
        }
    }
}
