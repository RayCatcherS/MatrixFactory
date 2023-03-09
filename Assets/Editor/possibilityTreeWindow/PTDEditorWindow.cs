using UnityEditor;
using UnityEngine.UIElements;

public class PTDEditorWindow : EditorWindow
{
    [MenuItem("Window/PST/TreeDebugger")]
    public static void ShowExample()
    {
        GetWindow<PTDEditorWindow>("Possibility Tree Debugger");
    }

    private void CreateGUI() {
        AddGraphView();

        AddStyles();
    }

    private void AddGraphView() {
        PTDView treeView = new PTDView();

        treeView.StretchToParentSize();

        rootVisualElement.Add(treeView);
    }

    private void AddStyles() {
        // foglio di stile UI
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("PossibilityTreeDebugger/PTVariables.uss");
        rootVisualElement.styleSheets.Add(styleSheet);
    }
}