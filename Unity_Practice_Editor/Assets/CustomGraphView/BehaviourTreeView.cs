using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using static UnityEngine.RectTransform;

public class BehaviourTreeView : GraphView
{
    private BehaviourTree currentBehaviourTree;

    private List<BehaviourNodeView> nodeViews = new List<BehaviourNodeView>();

    //private 

    public BehaviourTreeView(BehaviourTree tree)
    {
        // GraphView의 기본 설정
        this.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // 배경 그리드 추가
        GridBackground gridBackground = new GridBackground();
        Insert(0, gridBackground);
        gridBackground.StretchToParentSize();

        currentBehaviourTree = tree;

        // 1. 현재 선택된 Tree의 정보를 가져와야함
        // 2. BehaviourNode 정보를 바탕으로
        // 3. GraphView.Node를 생성하기
        // 4. 연결까지


        //if (tree.rootNode == null)
        //{
        //    BehaviourNode rootNode = CreateBehaviourNode(tree, "Root Node");

        //    tree.rootNode = rootNode;
        //    tree.nodeList.Add(rootNode);
        //}


        // TODO 위치 저장은 어떻게하지...??????/
        foreach(BehaviourNode node in tree.NodeList)
        {
            CreateNodeView(node);
        }


        graphViewChanged -= OnGraphViewChanged;
        graphViewChanged += OnGraphViewChanged;

        //ClearSelection();

        //Node rootNode = CreateNode(new Vector2(100f, 100f), "Root Node");
        //rootNode.capabilities &= ~Capabilities.Movable; // 이동 불가 설정
        //AddElement(rootNode);

        // 다른 노드 추가 및 에지 연결도 이곳에서 구현
    }


    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //Debug.LogWarning("그래프뷰 체인지");

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                BehaviourNodeView node = edge.output.node as BehaviourNodeView;


                //var node = edge.output.node as BehaviourNode;
            });
        }
        else if (graphViewChange.movedElements != null)
        {
            graphViewChange.movedElements.ForEach(element =>
            {
                BehaviourNodeView nodeView = element as BehaviourNodeView;

                Rect rect = nodeView.GetPosition();
                Vector2 newPosition = rect.position;

                nodeView.SetPosition(rect);

                BehaviourNode node = currentBehaviourTree.FindNode(nodeView.guid);
                node.position = newPosition;
            });
        }
        // 포지션 옮기기

        return graphViewChange;
    }

        
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);

        // 마우스 우 클릭 이벤트
        // TODO : 스크립터블 오브젝트 + Node 같이 생성하는 함수로 바꾸기
        evt.menu.InsertAction(0, $"Create Node/New Action Node", (action) => TestMouseRight(currentBehaviourTree, action.eventInfo.mousePosition));
    }


    private void TestMouseRight(BehaviourTree behaviourTree, Vector2 position)
    {
        string guid = GUID.Generate().ToString();

        BehaviourNode node = BehaviourNode.CreateBehaviourNode(behaviourTree, guid, position);

        CreateNodeView(node);

        //CreateNode(position);
    }



    private void CreateNodeView(BehaviourNode node)
    {
        BehaviourNodeView nodeView = new BehaviourNodeView(node.guid);
        nodeView.title = node.guid;
        nodeView.guid = node.guid;
        //nodeView.name = "Test New Node1";

        nodeView.SetPosition(new Rect(node.position, Vector2.zero));

        nodeView.RegisterCallback<MouseDownEvent>(evt => OnSelectedNode(evt, nodeView));

        

        AddElement(nodeView);

        nodeViews.Add(nodeView);
    }

    private void OnSelectedNode(MouseDownEvent evt, BehaviourNodeView nodeView)
    {
        if (evt.clickCount == 1)
        {
            Selection.activeObject = currentBehaviourTree.FindNode(nodeView.guid);

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
