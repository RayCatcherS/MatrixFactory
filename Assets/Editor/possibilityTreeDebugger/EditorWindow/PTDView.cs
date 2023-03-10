using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace PT.Windows {

    using DebugView;
    using Enumerations;

    public class PTDView : GraphView {
        public PTDView() {
            AddManipulators();

            AddGridBackground();

            CreateNode();

            AddStyles();
        }

        private void CreateNode() {
            PTDViewNode node = new PTDViewNode("Possibility Path Node", PTNodeType.InternalNode);
            node.Draw();

            AddElement(node);
        }

        private void AddManipulators() {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
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

}
