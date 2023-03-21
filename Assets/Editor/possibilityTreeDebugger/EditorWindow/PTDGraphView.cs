using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace PT.View {

    using DebugView;
    using Enumerations;
    using Global;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PTDGraphView : GraphView {
        public PTDGraphView() {
            AddManipulators();

            AddGridBackground();

            CreateNode();

            AddStyles();
        }

        public void CreateNode() {
            PTDNodeView node = new PTDNodeView("Possibility Path Node 1", PTNodeType.RootNode, Vector2.zero);
            PTDNodeView node2 = new PTDNodeView("Possibility Path Node 2", PTNodeType.InternalNode, Vector2.zero);


            UnityEditor.Experimental.GraphView.Edge edgeConnection = node2.parentPort.ConnectTo(node.forwardPort);


            AddElement(edgeConnection);
            AddElement(node);
            AddElement(node2);
        }

        private void AddManipulators() {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            

            this.AddManipulator(DrawGeneratedPossibilityTreeContextualMenu());
            this.AddManipulator(GeneratePossibilityTreeContextualMenu());
        }

        private IManipulator DrawGeneratedPossibilityTreeContextualMenu() {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Draw Tree", actionEvent => DrawGeneratedPossibilityTree())
            );

            return contextualMenuManipulator;
        }
        private IManipulator GeneratePossibilityTreeContextualMenu() {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Generate Possibility Tree", actionEvent => GeneratePossibilityTree())
            );

            return contextualMenuManipulator;
        }

        private void DrawGeneratedPossibilityTree() {
            GlobalPossibilityTree.GetGeneratedTree();
        }
        private void GeneratePossibilityTree() {
            GlobalPossibilityTree.GenerateTree();
            DrawGeneratedPossibilityTree();
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
