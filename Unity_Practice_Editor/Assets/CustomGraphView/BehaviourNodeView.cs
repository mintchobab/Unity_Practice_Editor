using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourNodeView : UnityEditor.Experimental.GraphView.Node
{
    //public BehaviourNodeView parentNode;
    //public BehaviourNodeView childNode;

    public Port inputPort;
    public Port outputPort;

    public string guid;

    public BehaviourNode Node { get; private set; }


    public BehaviourNode behaviourNode;


    public BehaviourNodeView(BehaviourNode inNode)
    {
        Node = inNode;

        //Node node = new Node { title = "New Node" };

        //node.SetPosition(new Rect(position, Vector2.zero));

        // Input Port
        // Capacity�� Single�̸� �� ���� Node���� ���� ����, Multi�� ������ ����
        inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(Node));
        inputContainer.Add(inputPort);

        // Output Port
        outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Node));
        outputContainer.Add(outputPort);





        //ObjectField objectField = new ObjectField
        //{
        //    label = "Behaviour Tree",
        //    objectType = typeof(BehaviourNode),
        //    allowSceneObjects = false // �� ��ü�� ������� ����
        //};

        //objectField.RegisterValueChangedCallback(evt =>
        //{
        //    behaviourNode = evt.newValue as BehaviourTree;
        //});

        //mainContainer.Add(objectField);






        //AddElement(node);
    }


}
