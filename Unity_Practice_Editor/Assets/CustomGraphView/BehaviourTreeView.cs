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
        // GraphView�� �⺻ ����
        this.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // ��� �׸��� �߰�
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
                Debug.LogWarning("2�� �̻��� ��Ȳ22");
            }

            foreach (GraphElement element in graphViewChange.movedElements)
            {
                BehaviourNodeView nodeView = element as BehaviourNodeView;

                Rect rect = nodeView.GetPosition();
                Vector2 newPosition = rect.position;

                nodeView.SetPosition(rect);

                BehaviourNode node = myBehaviourTree.FindNode(nodeView.guid);
                node.SetPosition(newPosition);
            }
        }
        else if (graphViewChange.edgesToCreate != null)
        {
            if (graphViewChange.edgesToCreate.Count > 1)
            {
                Debug.LogWarning("2�� �̻��� ��Ȳ11");
            }

            foreach(Edge edge in graphViewChange.edgesToCreate)
            {
                Debug.LogWarning("���� Ȯ��");

                // ��ǲ ����� �θ� �ƿ�ǲ�� �Ǿ����
                BehaviourNodeView inputNode = edge.input.node as BehaviourNodeView;
                BehaviourNodeView outputNode = edge.output.node as BehaviourNodeView;

                inputNode.Node.SetParentNode(outputNode.guid);
                outputNode.Node.AddChildNode(inputNode.guid);
            }
        }
        // ������ �������°͵� ���� ������
        else if (graphViewChange.elementsToRemove != null)
        {
            if (graphViewChange.elementsToRemove.Count > 1)
            {
                Debug.LogWarning("2�� �̻��� ��Ȳ33");
            }

            foreach (GraphElement element in graphViewChange.elementsToRemove)
            {
                if (element is Edge)
                {
                    Edge edge = element as Edge;

                    BehaviourNodeView inputNode = edge.input.node as BehaviourNodeView;
                    BehaviourNodeView outputNode = edge.output.node as BehaviourNodeView;

                    inputNode.Node.RemoveParnetNode();
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

        // ���콺 �� Ŭ�� �̺�Ʈ
        // TODO : ��ũ���ͺ� ������Ʈ + Node ���� �����ϴ� �Լ��� �ٲٱ�
        evt.menu.InsertAction(0, $"Create Node/Selector Node", (action) => CreateSelectorNode(myBehaviourTree, action.eventInfo.mousePosition));
        evt.menu.InsertAction(1, $"Create Node/Sequence Node", (action) => CreateSequenceNode(myBehaviourTree, action.eventInfo.mousePosition));
        evt.menu.InsertAction(2, $"Create Node/Action Node", (action) => CreateActionNode(myBehaviourTree, action.eventInfo.mousePosition));
    }





    // TODO : ��ġ�°� �����غ���
    private void CreateSelectorNode(BehaviourTree tree, Vector2 position)
    {
        SelectorNode node = tree.CreateSelectorNode();
        node.SetPosition(position);

        CreateNodeView(node);
    }


    private void CreateSequenceNode(BehaviourTree tree, Vector2 position)
    {
        SequenceNode node = tree.CreateSequenceNode();
        node.SetPosition(position);

        CreateNodeView(node);
    }



    private void CreateActionNode(BehaviourTree tree, Vector2 position)
    {
        ActionNode node = tree.CreateActionNode();
        node.SetPosition(position);

        CreateNodeView(node);
    }







    private void CreateNodeView(BehaviourNode node)
    {
        BehaviourNodeView nodeView = new BehaviourNodeView(node);
        nodeView.title = node.Guid;
        nodeView.guid = node.Guid;

        nodeView.SetPosition(new Rect(new Vector2(node.PosX, node.PosY), Vector2.zero));
        nodeView.RegisterCallback<MouseDownEvent>(evt => OnSelectedNode(evt, nodeView));

        if (myBehaviourTree.RootNode.Guid == node.Guid)
            nodeView.capabilities &= ~Capabilities.Deletable;

        nodeViewList.Add(nodeView);

        AddElement(nodeView);
    }







    private void CreateEdge(BehaviourNode node)
    {
        if (node.ChildNodeList.Count == 0)
            return;

        foreach (string childGuid in node.ChildNodeList)
        {
            //input, output port�� �˾ƾ���....

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

            // Ʈ������ ã��
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
