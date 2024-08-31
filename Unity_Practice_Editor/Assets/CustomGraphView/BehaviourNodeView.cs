using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourNodeView : UnityEditor.Experimental.GraphView.Node
{
    public Port inputPort;
    public Port outputPort;
    public BehaviourNode behaviourNode;

    public string guid;

    public BehaviourNode Node { get; private set; }

    private BehaviourTree tree;


    public BehaviourNodeView(BehaviourTree inTree, BehaviourNode inNode)
    {
        Node = inNode;
        tree = inTree;

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

        AddFields();
        AddProperties();

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


    private void AddFields()
    {
        FieldInfo[] fields = Node.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (FieldInfo field in fields)
        {
            Attribute nodeAttribute = Attribute.GetCustomAttribute(field, typeof(NodeFieldAttribute));

            if (nodeAttribute != null)
            {
                if (field.FieldType == typeof(float))
                {
                    CreateField<float, FloatField>(field, () => new FloatField(field.Name));
                } 
                else if (field.FieldType == typeof(int))
                {
                    CreateField<int, IntegerField>(field, () => new IntegerField(field.Name));
                }
                else if (field.FieldType == typeof(string))
                {
                    CreateField<string, TextField>(field, () => new TextField(field.Name));
                }
            }
        }


        void CreateField<TValue, TField>(FieldInfo field, Func<TField> createField) where TField : BaseField<TValue>
        {
            TField fieldView = createField.Invoke();
            fieldView.value = (TValue)field.GetValue(Node);

            fieldView.RegisterValueChangedCallback(evt =>
            {
                field.SetValue(Node, evt.newValue);
                EditorUtility.SetDirty(tree);
            });

            mainContainer.Add(fieldView);
        }
    }



    private void AddProperties()
    {

    }
}
