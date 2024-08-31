using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

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
            if (graphViewChange.movedElements.Count > 1)
            {
                Debug.LogWarning("2개 이상인 상황22");
            }

            foreach (GraphElement element in graphViewChange.movedElements)
            {
                BehaviourNodeView nodeView = element as BehaviourNodeView;

                Rect rect = nodeView.GetPosition();
                Vector2 newPosition = rect.position;

                nodeView.SetPosition(rect);

                BehaviourNode node = myBehaviourTree.FindNode(nodeView.guid);
                node.PosX = newPosition.x;
                node.PosY = newPosition.y;
            }
        }
        else if (graphViewChange.edgesToCreate != null)
        {
            if (graphViewChange.edgesToCreate.Count > 1)
            {
                Debug.LogWarning("2개 이상인 상황11");
            }

            foreach(Edge edge in graphViewChange.edgesToCreate)
            {
                Debug.LogWarning("여기 확인");

                // 인풋 노드의 부모가 아웃풋이 되어야함
                BehaviourNodeView inputNode = edge.input.node as BehaviourNodeView;
                BehaviourNodeView outputNode = edge.output.node as BehaviourNodeView;

                inputNode.Node.ParentNodeGuid = outputNode.guid;
                outputNode.Node.AddChildNode(inputNode.guid);
            }
        }
        // 엣지가 지워지는것도 같이 감지됨
        else if (graphViewChange.elementsToRemove != null)
        {
            //if (graphViewChange.elementsToRemove.Count > 1)
            //{
            //    Debug.LogWarning("2개 이상인 상황33");
            //}

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
        BuildContextualMenuInternal(typeof(CompositeNode), 0, "Create Node/Composite Node/", "Assets/CustomGraphView/CompositeNode");
        BuildContextualMenuInternal(typeof(DecoratorNode), 1, "Create Node/Decorator Node/", "Assets/CustomGraphView/DecoratorNode");
        BuildContextualMenuInternal(typeof(ActionNode), 2, "Create Node/Action Node/", "Assets/CustomGraphView/ActionNode");

        void BuildContextualMenuInternal(Type parentNodeType, int actionIndex, string actionName, string folderPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:MonoScript", new[] { folderPath });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

                if (script == null)
                    continue;

                System.Type scriptType = script.GetClass();

                if (scriptType != null && scriptType.IsSubclassOf(parentNodeType))
                {
                    evt.menu.InsertAction(actionIndex, $"{actionName}{scriptType.Name}", (action) => OnCreatedNode(scriptType, myBehaviourTree, action.eventInfo.mousePosition));
                }
            }
        }
    }


    private void OnCreatedNode(Type nodeType, BehaviourTree tree, Vector2 position)
    {
        BehaviourNode node = tree.CreateNode(nodeType);
        node.PosX = position.x;
        node.PosY = position.y;

        CreateNodeView(node);
    }


    private void CreateNodeView(BehaviourNode node)
    {
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
        if (node.ChildNodeGuidList.Count == 0)
            return;

        foreach (string childGuid in node.ChildNodeGuidList)
        {
            //input, output port를 알아야함....

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
        if (evt.clickCount == 1)
        {
            //Selection.activeObject = myBehaviourTree.FindNode(nodeView.guid);

            // 트리에서 찾기
            //Debug.Log($"{nodeView.guid}");
        }
    }


    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()!.Where(endPort =>
                      endPort.direction != startPort.direction &&
                      endPort.node != startPort.node &&
                      endPort.portType == startPort.portType).ToList();
    }
}
