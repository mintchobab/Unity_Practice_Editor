using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourNodeView : UnityEditor.Experimental.GraphView.Node
{
    public Port inputPort;
    public Port outputPort;
    public BehaviourNode behaviourNode;

    public string guid;

    public BehaviourNode Node { get; private set; }


    public BehaviourNodeView(BehaviourNode inNode)
    {
        Node = inNode;

        style.width = 200;
        StyleColor inputColor = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 체크박스 삭제
        var collapseButton = titleContainer.Q("collapse-button");
        if (collapseButton != null)
        {
            collapseButton.RemoveFromHierarchy();
        }

        if (Node is RootNode)
        {
            outputPort = AddOutputPort(Port.Capacity.Single);
        }
        else if (Node is CompositeNode)
        {
            inputColor = new Color(129f / 255f, 44f / 255f, 27f / 255f, 1f);

            inputPort = AddInputPort(Port.Capacity.Single);
            outputPort = AddOutputPort(Port.Capacity.Multi);
        }
        else if (Node is DecoratorNode)
        {
            inputColor = new Color(0f / 255f, 132f / 255f, 43f / 255f, 1f);

            inputPort = AddInputPort(Port.Capacity.Single);
            outputPort = AddOutputPort(Port.Capacity.Multi);
        }
        else if (Node is ActionNode)
        {
            inputColor = new Color(23f / 255f, 64f / 255f, 165f / 255f, 1f);

            inputPort = AddInputPort(Port.Capacity.Single);
        }
        else
        {
            Debug.LogError($"{nameof(BehaviourNodeView)} : Node Error {typeof(Node)}");
        }

        title = Node.GetType().Name;

        titleContainer.style.backgroundColor = inputColor;
        titleContainer.style.justifyContent = Justify.Center;

        var titleLabel = titleContainer.Q<Label>("title-label");
        if (titleLabel != null)
            titleLabel.style.color = Color.white;

        inputContainer.Add(inputPort);
        outputContainer.Add(outputPort);

        RefreshExpandedState();
        RefreshPorts();
    }


    private Port AddInputPort(Port.Capacity portCapacity)
    {
        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, portCapacity, typeof(float));
        inputPort.portName = "Input";

        return inputPort;
    }


    private Port AddOutputPort(Port.Capacity portCapacity)
    {
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, portCapacity, typeof(float));
        outputPort.portName = "Output";

        return outputPort;
    }
}
