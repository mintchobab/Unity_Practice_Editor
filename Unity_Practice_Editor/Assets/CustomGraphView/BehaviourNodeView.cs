using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BehaviourNodeView : UnityEditor.Experimental.GraphView.Node
{
    public BehaviourNodeView parentNode;
    public BehaviourNodeView childNode;

    public Port inputPort;
    public Port outputPort;

    public string guid;

    public BehaviourNodeView(string guid)
    {
        //Node node = new Node { title = "New Node" };

        //node.SetPosition(new Rect(position, Vector2.zero));

        // Input Port
        // Capacity�� Single�̸� �� ���� Node���� ���� ����, Multi�� ������ ����
        inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(Node));
        inputContainer.Add(inputPort);

        // Output Port
        outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Node));
        outputContainer.Add(outputPort);

        //AddElement(node);
    }
}
