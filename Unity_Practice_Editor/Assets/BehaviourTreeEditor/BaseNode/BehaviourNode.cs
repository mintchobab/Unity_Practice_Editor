using System.Collections.Generic;
using UnityEngine;

namespace Mintchobab
{
    public enum NodeStates
    {
        None,
        Running,
        Success,
        Failure
    }


    [System.Serializable]
    public abstract class BehaviourNode
    {
        [SerializeReference]
        protected string guid;
        public string Guid
        {
            get => guid;
            set => guid = value;
        }


        [SerializeReference]
        protected string parentNodeGuid;
        public string ParentNodeGuid
        {
            get => parentNodeGuid;
            set => parentNodeGuid = value;
        }


        [SerializeReference]
        protected List<string> childNodeGuidList = new List<string>();
        public List<string> ChildNodeGuidList
        {
            get => childNodeGuidList;
            set => childNodeGuidList = value;
        }


        [SerializeReference]
        protected float posX;
        public float PosX
        {
            get => posX;
            set => posX = value;
        }


        [SerializeReference]
        protected float posY;
        public float PosY
        {
            get => posY;
            set => posY = value;
        }


        public BehaviourNode(string inGuid)
        {
            this.guid = inGuid;
        }


        protected BehaviourTree tree;

        protected List<BehaviourNode> childNodes;

        public NodeStates NodeState { get; set; } = NodeStates.None;


        public void AddChildNode(string inGuid)
        {
            if (childNodeGuidList.Contains(inGuid))
                return;

            childNodeGuidList.Add(inGuid);
        }


        public void RemoveChildNode(string inGuid)
        {
            if (!childNodeGuidList.Contains(inGuid))
                return;

            childNodeGuidList.Remove(inGuid);
        }

        public void SortChildNodeByPositionY(BehaviourTree inTree)
        {
            childNodeGuidList.Sort((a, b) => inTree.FindNode(a).PosY.CompareTo(inTree.FindNode(b).PosY));
        }


        public virtual void Init(BehaviourTree tree)
        {
            this.tree = tree;
            NodeState = NodeStates.None;

            childNodes = new List<BehaviourNode>();

            foreach (string nodeGuid in childNodeGuidList)
            {
                BehaviourNode node = tree.FindNode(nodeGuid);

                if (node == null)
                {
                    Debug.LogError($"{nameof(SelectorNode)} : Child Node Not Found");
                    continue;
                }

                childNodes.Add(node);
            }
        }

        public virtual void Refresh() 
        {
            NodeState = NodeStates.None;
        }

        public abstract NodeStates Evaluate();               
    }
}
