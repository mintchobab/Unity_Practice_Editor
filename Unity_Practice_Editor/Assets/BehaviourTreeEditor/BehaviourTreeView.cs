using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Mintchobab
{
    public class BehaviourTreeView : GraphView
    {
        private BehaviourTree myBehaviourTree;
        private List<BehaviourNodeView> nodeViewList = new List<BehaviourNodeView>();

        public BehaviourTreeView(BehaviourTree tree)
        {
            // GraphView의 기본 설정
            this.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // 배경 그리드 추가
            styleSheets.Add(Resources.Load<StyleSheet>("Background"));
            GridBackground gridBackground = new GridBackground();
            Insert(0, gridBackground);
            gridBackground.StretchToParentSize();

            myBehaviourTree = tree;
            nodeViewList.Clear();

            foreach (BehaviourNode node in myBehaviourTree.NodeList)
            {
                CreateNodeView(node);
            }

            foreach (BehaviourNode node in myBehaviourTree.NodeList)
            {
                CreateEdge(node);
            }

            graphViewChanged -= OnGraphViewChanged;
            graphViewChanged += OnGraphViewChanged;
        }


        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                List<string> parentNodeGuidList = new List<string>();

                foreach (GraphElement element in graphViewChange.movedElements)
                {
                    BehaviourNodeView nodeView = element as BehaviourNodeView;

                    Rect rect = nodeView.GetPosition();
                    Vector2 newPosition = rect.position;

                    nodeView.SetPosition(rect);

                    BehaviourNode node = myBehaviourTree.FindNode(nodeView.guid);
                    node.PosX = newPosition.x;
                    node.PosY = newPosition.y;

                    if (!string.IsNullOrEmpty(node.ParentNodeGuid) && !parentNodeGuidList.Contains(node.ParentNodeGuid))
                        parentNodeGuidList.Add(node.ParentNodeGuid);
                }

                // 정렬 중복하지 않기 위해 사용
                foreach (string parentGuid in parentNodeGuidList)
                {
                    BehaviourNode parentNode = myBehaviourTree.FindNode(parentGuid);
                    parentNode.SortChildNodeByPositionY(myBehaviourTree);
                }
            }
            else if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    BehaviourNodeView inputNode = edge.input.node as BehaviourNodeView;
                    BehaviourNodeView outputNode = edge.output.node as BehaviourNodeView;

                    inputNode.Node.ParentNodeGuid = outputNode.guid;
                    outputNode.Node.AddChildNode(inputNode.guid);
                    outputNode.Node.SortChildNodeByPositionY(myBehaviourTree);
                }
            }
            // NOTE : 엣지가 지워지는것도 같이 감지됨
            else if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    if (element is Edge)
                    {
                        Edge edge = element as Edge;

                        BehaviourNodeView inputNode = edge.input.node as BehaviourNodeView;
                        BehaviourNodeView outputNode = edge.output.node as BehaviourNodeView;

                        inputNode.Node.ParentNodeGuid = null;
                        outputNode.Node.RemoveChildNode(inputNode.guid);
                    }
                    else if (element is BehaviourNodeView)
                    {
                        BehaviourNodeView nodeView = element as BehaviourNodeView;
                        myBehaviourTree.RemoveNode(nodeView.Node);
                    }
                }
            }

            EditorUtility.SetDirty(myBehaviourTree);

            return graphViewChange;
        }


        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            // 마우스 우 클릭 이벤트
            BuildContextualMenuInternal<CompositeNode>(0, "Create Node/Composite Node");
            BuildContextualMenuInternal<DecoratorNode>(1, "Create Node/Decorator Node");
            BuildContextualMenuInternal<ActionNode>(2, "Create Node/Action Node");
            BuildContextualMenuInternal<ConditionNode>(3, "Create Node/Condition Node");

            void BuildContextualMenuInternal<TNode>(int actionIndex, string actionName) where TNode : BehaviourNode
            {
                var nodeTypeList = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsClass && type.IsSubclassOf(typeof(TNode)))
                    .ToList();

                if (nodeTypeList.Count == 0)
                    return;

                foreach (Type nodeType in nodeTypeList)
                {
                    evt.menu.InsertAction(actionIndex, $"{actionName}/{nodeType.Name}", (action) =>
                    {
                        BehaviourNode node = myBehaviourTree.CreateNode(nodeType);
                        node.PosX = action.eventInfo.mousePosition.x;
                        node.PosY = action.eventInfo.mousePosition.y;

                        CreateNodeView(node);
                    });
                }
            }
        }


        private void CreateNodeView(BehaviourNode node)
        {
            if (node == null)
            {
                Debug.LogError($"{nameof(BehaviourTreeView)} : Node is Null");
                return;
            }

            BehaviourNodeView nodeView = new BehaviourNodeView(myBehaviourTree, node);
            nodeView.guid = node.Guid;

            nodeView.SetPosition(new Rect(new Vector2(node.PosX, node.PosY), Vector2.zero));
            nodeView.RegisterCallback<MouseDownEvent>(evt => OnSelectedNode(evt, nodeView));

            if (node is RootNode)
                nodeView.capabilities &= ~Capabilities.Deletable;

            nodeViewList.Add(nodeView);

            AddElement(nodeView);
        }


        private void CreateEdge(BehaviourNode node)
        {
            if (node == null)
            {
                Debug.LogError($"{nameof(BehaviourTreeView)} : Node is Null");
                return;
            }

            if (node.ChildNodeGuidList.Count == 0)
                return;

            foreach (string childGuid in node.ChildNodeGuidList)
            {
                BehaviourNodeView outputNodeView = nodeViewList.Find(x => x.guid.Equals(node.Guid));
                BehaviourNodeView inputNodeView = nodeViewList.Find(x => x.guid.Equals(childGuid));

                Edge edge = new Edge();
                edge.output = outputNodeView.outputPort;
                edge.input = inputNodeView.inputPort;

                edge.output.Connect(edge);
                edge.input.Connect(edge);

                AddElement(edge);
            }
        }


        private void OnSelectedNode(MouseDownEvent evt, BehaviourNodeView nodeView)
        {

        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()!.Where(endPort =>
                          endPort.direction != startPort.direction &&
                          endPort.node != startPort.node &&
                          endPort.portType == startPort.portType).ToList();
        }
    }
}
