using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mintchobab
{
    [CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "ScriptableObjects/Behaviour Tree", order = 1)]
    public class BehaviourTree : ScriptableObject
    {
        [SerializeReference]
        public List<BehaviourNode> NodeList = new List<BehaviourNode>();

        public string RootNodeGuid;

        private BehaviourNode rootNode;

        public BehaviourTreeBlackboard Blackboard { get; private set; }


        private void OnEnable()
        {
            if (RootNodeGuid == null)
            {
                BehaviourNode node = CreateNode(typeof(RootNode));
                RootNodeGuid = node.Guid;
            }
        }


        public BehaviourNode CreateNode(Type nodeType)
        {
            string guid = System.Guid.NewGuid().ToString();

            BehaviourNode newNode = Activator.CreateInstance(nodeType, guid) as BehaviourNode;

            NodeList.Add(newNode);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            return newNode;
        }


        public void RemoveNode(BehaviourNode node)
        {
            if (!NodeList.Contains(node))
                return;

            if (node.Guid == RootNodeGuid)
                return;

            NodeList.Remove(node);

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }


        public BehaviourNode FindNode(string guid)
        {
            return NodeList.Find(x => x.Guid == guid);
        }


        public void Init(BehaviourTreeBlackboard blackboard)
        {
            this.Blackboard = blackboard;

            if (string.IsNullOrEmpty(RootNodeGuid))
                Debug.LogError($"{nameof(BehaviourTree)} : Root Node Guid is Empty");

            rootNode = FindNode(RootNodeGuid);

            InitRecursive(rootNode);
        }

        private void InitRecursive(BehaviourNode node)
        {
            node.Init(this);

            List<string> nodeGuidList = FindNode(node.Guid).ChildNodeGuidList;

            foreach (string guid in nodeGuidList)
                InitRecursive(FindNode(guid));
        }


        public void Evaluate()
        {
            rootNode.Evaluate();
        }
    }
}
