using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace PT.View {

    using DebugView;
    using Enumerations;
    using Global;
    using PT.DataStruct;
    using System;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PTDGraphView : GraphView {
        public PTDGraphView() {
            AddManipulators();

            AddGridBackground();

            //GenerateNodeByTree(GlobalPossibilityTree.GetGeneratedTree());

            AddStyles();
        }

        public void GenerateNodeByTree(FourCTree<PossibilityItem> tree) {

            Dictionary<string, PTDNodeView> nodeViewDictionary =
            new Dictionary<string, PTDNodeView>();

            Action<FourCTreeNode<PossibilityItem>, FourCTree<PossibilityItem>> onNodeVisit =
            (FourCTreeNode<PossibilityItem> visitedNode, FourCTree<PossibilityItem> tree) => {


                PTDNodeView visitedNodeView = new PTDNodeView(
                    "Possibility Path Node: " + visitedNode.getItem(),
                    tree.isRoot(visitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                    Vector2.zero
                );
                if(tree.isRoot(visitedNode)) {
                    nodeViewDictionary.Add(tree.Read(visitedNode).id, visitedNodeView);
                    AddElement(visitedNodeView);
                }
                


                if (!tree.ForwardIsEmpty(visitedNode)) {

                    FourCTreeNode<PossibilityItem> fVisitedNode = tree.Forward(visitedNode);
                    PTDNodeView visitedFNodeView = new PTDNodeView(
                        "Possibility Path Node: " + fVisitedNode.getItem(),
                        tree.isRoot(fVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );
                    PTDNodeView fParentNodeView = nodeViewDictionary[tree.Read(tree.Parent(fVisitedNode)).id];
                    var edgeConnection = visitedFNodeView.parentPort.ConnectTo(
                        fParentNodeView.forwardPort
                     );

                    nodeViewDictionary.Add(tree.Read(fVisitedNode).id, fParentNodeView);

                    AddElement(fParentNodeView);
                    AddElement(edgeConnection);
                }


                if (!tree.LeftIsEmpty(visitedNode))
                {

                    FourCTreeNode<PossibilityItem> lVisitedNode = tree.Left(visitedNode);
                    PTDNodeView visitedFNodeView = new PTDNodeView(
                        "Possibility Path Node: " + lVisitedNode.getItem(),
                        tree.isRoot(lVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );
                    PTDNodeView lParentNodeView = nodeViewDictionary[tree.Read(tree.Parent(lVisitedNode)).id];
                    var edgeConnection = visitedFNodeView.parentPort.ConnectTo(
                        lParentNodeView.leftPort
                     );

                    nodeViewDictionary.Add(tree.Read(lVisitedNode).id, lParentNodeView);

                    
                    AddElement(lParentNodeView);
                    AddElement(edgeConnection);
                }

                //Debug.Log(visitedNode.getItem().ToString());
            };
            tree.VisitTree(tree.Root(), onNodeVisit);
            
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
            GenerateNodeByTree(GlobalPossibilityTree.GetGeneratedTree());
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
