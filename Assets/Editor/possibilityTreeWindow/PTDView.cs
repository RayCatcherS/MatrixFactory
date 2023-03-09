using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class PTDView : GraphView
{
    public PTDView() {
        AddGridBackground();

        AddStyles();
    }

    private void AddGridBackground() {
        GridBackground gridBackground = new GridBackground();

        

        //gridBackground.styleSheets.Add(styleSheet);
        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }

    private void AddStyles() {
        // foglio di stile UI
        StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("PossibilityTreeDebugger/PTDViewStyles.uss");
        styleSheets.Add(styleSheet);
    }
}
