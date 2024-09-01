using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditorWindow : GraphViewEditorWindow
{
    private BehaviourTreeView graphView;
    private TwoPaneSplitView splitView;
    private VisualElement leftPanel;
    private VisualElement rightPanel;


    [MenuItem("Custom/Behaviour Tree/Tree Editor")]
    public static void OpenWindow()
    {
        BehaviourTreeEditorWindow window = GetWindow<BehaviourTreeEditorWindow>();
        window.titleContent = new GUIContent("Behaviour Tree Editor");
        window.Show();
    }


    private void OnEnable()
    {
        rootVisualElement.Clear();

        SetupLayout();

        Selection.selectionChanged += OnSelectionChanged;
        OnSelectionChanged();
    }


    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }


    private void SetupLayout()
    {
        // Split View
        splitView = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        // ���� �г�
        leftPanel = new VisualElement();
        splitView.Add(leftPanel);

        // ���� �г�
        rightPanel = new VisualElement();
        rightPanel.style.flexDirection = FlexDirection.Column; // ���� ���̾ƿ� ����
        splitView.Add(rightPanel);

        // ����
        Toolbar toolbar = new Toolbar();
        toolbar.style.justifyContent = Justify.FlexEnd;


        ToolbarButton refreshButton = new ToolbarButton(() => OnSelectionChanged());
        ToolbarButton saveButton = new ToolbarButton(OnClickedSaveButton);

        refreshButton.text = "Refresh";
        saveButton.text = "Save";

        toolbar.Add(refreshButton);
        toolbar.Add(saveButton);

        rightPanel.Add(toolbar);
    }


    private void OnClickedSaveButton()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    private void OnSelectionChanged()
    {
        if (Selection.activeObject is BehaviourTree)
        {
            if (graphView != null && rightPanel != null)
            {
                rightPanel.Remove(graphView);
            }

            BehaviourTree tree = Selection.activeObject as BehaviourTree;

            graphView = new BehaviourTreeView(tree);

            // �ȵ���̵� ��Ʃ����� weight ���� ȿ��
            graphView.style.flexGrow = 1;

            rightPanel.Add(graphView);
        }
    }
}
