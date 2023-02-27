using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class PossibilityTreeView : EditorWindow
{
    [MenuItem("Window/UI Toolkit/PossibilityTreeView")]
    public static void ShowExample()
    {
        PossibilityTreeView wnd = GetWindow<PossibilityTreeView>();
        wnd.titleContent = new GUIContent("PossibilityTreeView");
    }

    public void CreateGUI()
    {
        VisualElement container = new VisualElement();

        // if "Null" allora il visual element non viene renderezzato in nessun visual tree
        Debug.Log(container.panel);
    }
}