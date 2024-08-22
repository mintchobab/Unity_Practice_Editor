using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class BehaviourTreeView : GraphView
{
    private BehaviourTree myBehaviourTree;


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

        myBehaviourTree = tree;

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
    }


    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.edgesToCreate != null)
        {
            Debug.LogWarning("엣지 개수 테스트 : " + graphViewChange.edgesToCreate.Count);

            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                // 인풋 노드의 부모가 아웃풋이 되어야함
                BehaviourNodeView inputNode = edge.input.node as BehaviourNodeView;
                BehaviourNodeView outputNode = edge.output.node as BehaviourNodeView;

                inputNode.ChangeEdge(outputNode, inputNode);
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

                BehaviourNode node = myBehaviourTree.FindNode(nodeView.guid);
                node.SetPosition(newPosition);
            });
        }
        else if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                BehaviourNodeView nodeView = element as BehaviourNodeView;

                myBehaviourTree.DestroyBehaviourNode(nodeView.MyNode);
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
        evt.menu.InsertAction(0, $"Create Node/New Action Node", (action) => TestMouseRight(myBehaviourTree, action.eventInfo.mousePosition));
    }


    private void TestMouseRight(BehaviourTree behaviourTree, Vector2 position)
    {
        string guid = GUID.Generate().ToString();

        BehaviourNode node = behaviourTree.CreateBehaviourNode(guid, position);

        CreateNodeView(node);

        //CreateNode(position);
    }



    private void CreateNodeView(BehaviourNode node)
    {
        BehaviourNodeView nodeView = new BehaviourNodeView(node);
        nodeView.title = node.guid;
        nodeView.guid = node.guid;
        //nodeView.name = "Test New Node1";

        nodeView.SetPosition(new Rect(node.position, Vector2.zero));

        nodeView.RegisterCallback<MouseDownEvent>(evt => OnSelectedNode(evt, nodeView));        

        AddElement(nodeView);
    }

    private void OnSelectedNode(MouseDownEvent evt, BehaviourNodeView nodeView)
    {
        if (evt.clickCount == 1)
        {
            Selection.activeObject = myBehaviourTree.FindNode(nodeView.guid);

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
