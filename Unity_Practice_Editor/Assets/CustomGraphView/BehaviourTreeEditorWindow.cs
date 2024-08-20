using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditorWindow : GraphViewEditorWindow
{
    private BehaviourTreeView graphView;

    [MenuItem("Custom/Behaviour Tree/Tree Editor")]
    public static void OpenWindow()
    {
        BehaviourTreeEditorWindow window = GetWindow<BehaviourTreeEditorWindow>();
        window.titleContent = new GUIContent("Behaviour Tree Editor");
        window.Show();
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;

        OnSelectionChanged();
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        if (Selection.activeObject is BehaviourTree)
        {
            if (graphView != null)
            {
                rootVisualElement.Remove(graphView);
            }

            graphView = new BehaviourTreeView(Selection.activeObject as BehaviourTree);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
    }
}
