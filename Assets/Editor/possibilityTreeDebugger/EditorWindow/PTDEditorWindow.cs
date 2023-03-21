using PT.View;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;

namespace PT.View {
    public class PTDEditorWindow : EditorWindow {
        [MenuItem("Window/PT/TreeDebugger")]
        public static void ShowExample() {
            GetWindow<PTDEditorWindow>("Possibility Tree Debugger");
        }

        private void CreateGUI() {
            AddGraphView();
            AddStyles();
        }

        private void AddGraphView() {
            PTDGraphView treeView = new PTDGraphView();

            treeView.StretchToParentSize();

            rootVisualElement.Add(treeView);
        }

        private void AddStyles() {
            // foglio di stile UI
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("PossibilityTreeDebugger/PTVariables.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}