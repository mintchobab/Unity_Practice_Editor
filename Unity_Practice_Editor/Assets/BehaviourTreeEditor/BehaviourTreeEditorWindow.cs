using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditorWindow : GraphViewEditorWindow
{
    private BehaviourTreeView graphView;
    private TwoPaneSplitView splitView;

    private static VisualElement leftPanel;
    private static VisualElement rightPanel;


    public static Texture2D testTexture2D;


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
        splitView = new TwoPaneSplitView(0, 300, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        SetUpLeftPanel();
        SetUpRightPanel();
    }

    private void SetUpLeftPanel()
    {
        // 좌측 패널
        leftPanel = new VisualElement();
        leftPanel.style.minWidth = 300f;

        VisualElement leftUpContainer = new VisualElement();
        VisualElement leftDownContainer = new VisualElement();
        leftDownContainer.style.flexGrow = 1;
        leftUpContainer.style.flexGrow = 1;        

        VisualElement tabContainer = new VisualElement();
        tabContainer.style.flexDirection = FlexDirection.Row; // 가로로 요소 배치
        tabContainer.style.justifyContent = Justify.FlexStart; // 왼쪽 정렬
        tabContainer.style.alignItems = Align.Center; // 수직 중앙 정렬

        // Setting Button
        Button tabButton = new Button(() =>
        {
            Debug.LogWarning("테스트 버튼");
        });

        tabButton.style.flexGrow = 1;
        tabButton.style.marginLeft = 0;
        tabButton.style.marginRight = 0;

        tabContainer.Add(tabButton);


        // Test Button
        Button tabButton2 = new Button();
        tabButton2.style.flexGrow = 1;
        tabButton2.style.marginLeft = 0;
        tabButton2.style.marginRight = 0;

        tabContainer.Add(tabButton2);

        VisualElement tabContentsContainer = new VisualElement();
        ObjectField textureField = new ObjectField()
        {
            objectType = typeof(Texture2D)
        };

        textureField.RegisterValueChangedCallback(evt =>
        {
            testTexture2D = (Texture2D)evt.newValue;
        });

        tabContentsContainer.Add(textureField);


        leftUpContainer.Add(tabContainer);
        leftUpContainer.Add(tabContentsContainer);


        // 수평선
        Box horizontalLine = new Box();

        horizontalLine.style.height = 1;
        horizontalLine.style.width = Length.Percent(100);
        horizontalLine.style.backgroundColor = new StyleColor(Color.black);        

        leftDownContainer.Add(horizontalLine);

        leftDownContainer.Add(new TextField("Test"));

        leftPanel.Add(leftUpContainer);
        leftPanel.Add(leftDownContainer);

        splitView.Add(leftPanel);
    }


    private void SetUpRightPanel()
    {
        // 우측 패널
        rightPanel = new VisualElement();
        rightPanel.style.flexDirection = FlexDirection.Column; // 수직 레이아웃 설정

        splitView.Add(rightPanel);

        // 툴바
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

            // 안드로이드 스튜디오의 weight 같은 효과
            graphView.style.flexGrow = 1;

            rightPanel.Add(graphView);
        }
    }


    public static void UpdateLeftPanel(BehaviourNodeView nodeView)
    {
        leftPanel.Clear();
        leftPanel.Add(new TextField("Guid") { value = nodeView.guid });
    }

    public static void ClearLeftPanel()
    {
        leftPanel.Clear();
    }
}
