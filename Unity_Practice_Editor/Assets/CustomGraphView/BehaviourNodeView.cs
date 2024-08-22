using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BehaviourNodeView : UnityEditor.Experimental.GraphView.Node
{
    public BehaviourNodeView parentNode;
    public BehaviourNodeView childNode;

    public Port inputPort;
    public Port outputPort;

    public string guid;

    public BehaviourNode MyNode { get; private set; }


    public BehaviourNodeView(BehaviourNode node)
    {
        MyNode = node;

        //Node node = new Node { title = "New Node" };

        //node.SetPosition(new Rect(position, Vector2.zero));

        // Input Port
        // Capacity가 Single이면 한 개의 Node랑만 연결 가능, Multi는 여러개 가능
        inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(Node));
        inputContainer.Add(inputPort);

        // Output Port
        outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Node));
        outputContainer.Add(outputPort);

        //AddElement(node);
    }


    public void ChangeEdge(BehaviourNodeView parentNode, BehaviourNodeView childNode)
    {
        if (parentNode == null || childNode == null)
        {
            Debug.LogWarning("노드 없음>>..???");
        }

        MyNode.SetParentNode(parentNode.MyNode);
        MyNode.AddChildNode(childNode.MyNode);
    }
}
