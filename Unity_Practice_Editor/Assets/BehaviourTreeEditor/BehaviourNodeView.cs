using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
#endif

namespace Mintchobab
{
#if UNITY_EDITOR
    public class BehaviourNodeView : Node
    {
        public Port inputPort;
        public Port outputPort;
        public BehaviourNode behaviourNode;

        public string guid;

        private BehaviourTree tree;

        public BehaviourNode Node { get; private set; }


        public BehaviourNodeView(BehaviourTree inTree, BehaviourNode inNode)
        {
            Node = inNode;
            tree = inTree;

            if (Node == null)
            {
                Debug.LogError($"{nameof(BehaviourNodeView)} : Node is Null");
                return;
            }

            style.flexGrow = 1f;
            style.minWidth = 200f;

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
                outputPort = AddOutputPort(Port.Capacity.Single);
            }
            else if (Node is ActionNode)
            {
                inputColor = new Color(23f / 255f, 64f / 255f, 165f / 255f, 1f);

                inputPort = AddInputPort(Port.Capacity.Single);
            }
            else if (Node is ConditionNode)
            {
                inputColor = new Color(142f / 255f, 44f / 255f, 199f / 255f, 1f);

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

            extensionContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            extensionContainer.style.overflow = Overflow.Visible;

            // NOTE : 순서 주의하기
            AddNodeImage();

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


        private void AddNodeImage()
        {
            Image image = new Image();

            if (Node is SequenceNode)
            {
                image.image = BehaviourTreeSettings.Instance.SequenceTexture;
            }
            else if (Node is SelectorNode)
            {
                image.image = BehaviourTreeSettings.Instance.SelectorTexture;
            }
            else if (Node is WaitNode)
            {
                image.image = BehaviourTreeSettings.Instance.WaitTexture;
            }

            if (image.image == null)
                return;

            image.style.width = 70;
            image.style.height = 70;
            image.style.alignSelf = Align.Center;

            extensionContainer.Add(image);
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
                        CreateFieldView<float, FloatField>(field, () => new FloatField(field.Name));
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        CreateFieldView<int, IntegerField>(field, () => new IntegerField(field.Name));
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        CreateFieldView<string, TextField>(field, () => new TextField(field.Name));
                    }
                }
            }

            void CreateFieldView<TValue, TField>(FieldInfo fieldInfo, Func<TField> createFunc) where TField : BaseField<TValue>
            {
                TField fieldView = createFunc.Invoke();
                fieldView.value = (TValue)fieldInfo.GetValue(Node);

                fieldView.RegisterValueChangedCallback(evt =>
                {
                    fieldInfo.SetValue(Node, evt.newValue);
                    EditorUtility.SetDirty(tree);
                });

                extensionContainer.Add(fieldView);
            }
        }

        private void AddProperties()
        {
            PropertyInfo[] properties = Node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (PropertyInfo property in properties)
            {
                Attribute nodeAttribute = Attribute.GetCustomAttribute(property, typeof(NodePropertyAttribute));

                if (nodeAttribute != null)
                {
                    if (property.PropertyType == typeof(float))
                    {
                        CreatePropertyView<float, FloatField>(property, () => new FloatField(property.Name));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        CreatePropertyView<int, IntegerField>(property, () => new IntegerField(property.Name));
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        CreatePropertyView<string, TextField>(property, () => new TextField(property.Name));
                    }
                }
            }

            void CreatePropertyView<TValue, TField>(PropertyInfo propertyInfo, Func<TField> createFunc) where TField : BaseField<TValue>
            {
                TField fieldView = createFunc.Invoke();
                fieldView.value = (TValue)propertyInfo.GetValue(Node);

                fieldView.RegisterValueChangedCallback(evt =>
                {
                    propertyInfo.SetValue(Node, evt.newValue);
                    EditorUtility.SetDirty(tree);
                });

                extensionContainer.Add(fieldView);
            }
        }
    }
#endif
}
