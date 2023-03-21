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


                PTDNodeView visitedNodeView;
                if(tree.isRoot(visitedNode)) {
                    visitedNodeView = new PTDNodeView(
                        "Possibility Path Node: " + visitedNode.getItem(),
                        tree.isRoot(visitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    ); 
                    //nodeViewDictionary.Add(tree.Read(visitedNode).id, visitedNodeView);
                    AddElement(visitedNodeView);
                } else {
                    visitedNodeView = nodeViewDictionary[tree.Read(visitedNode).id];
                    //AddElement(fParentNodeView);

                }


                if(!tree.ForwardIsEmpty(visitedNode)) {
                    FourCTreeNode<PossibilityItem> fVisitedNode = tree.Forward(visitedNode); // nodo figlio forward
                    PTDNodeView fVisitedNodeView = new PTDNodeView( // render nodo figlio forward
                        "Possibility Path Node: " + fVisitedNode.getItem(),
                        tree.isRoot(fVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );
                    nodeViewDictionary.Add(tree.Read(fVisitedNode).id, fVisitedNodeView); // add render nodo figlio forward dictionary

                    var edgeConnection = fVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.forwardPort
                     );
                    AddElement(fVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.BackIsEmpty(visitedNode)) {
                    FourCTreeNode<PossibilityItem> bVisitedNode = tree.Back(visitedNode); // nodo figlio forward
                    PTDNodeView bVisitedNodeView = new PTDNodeView( // render nodo figlio forward
                        "Possibility Path Node: " + bVisitedNode.getItem(),
                        tree.isRoot(bVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );
                    nodeViewDictionary.Add(tree.Read(bVisitedNode).id, bVisitedNodeView); // add render nodo figlio forward dictionary

                    var edgeConnection = bVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.backPort
                     );
                    AddElement(bVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.RightIsEmpty(visitedNode)) {
                    FourCTreeNode<PossibilityItem> rVisitedNode = tree.Right(visitedNode); // nodo figlio forward
                    PTDNodeView rVisitedNodeView = new PTDNodeView( // render nodo figlio forward
                        "Possibility Path Node: " + rVisitedNode.getItem(),
                        tree.isRoot(rVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );
                    nodeViewDictionary.Add(tree.Read(rVisitedNode).id, rVisitedNodeView); // add render nodo figlio forward dictionary

                    var edgeConnection = rVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.rightPort
                     );
                    AddElement(rVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.LeftIsEmpty(visitedNode)) {
                    FourCTreeNode<PossibilityItem> lVisitedNode = tree.Left(visitedNode); // nodo figlio forward
                    PTDNodeView lVisitedNodeView = new PTDNodeView( // render nodo figlio forward
                        "Possibility Path Node: " + lVisitedNode.getItem(),
                        tree.isRoot(lVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );
                    nodeViewDictionary.Add(tree.Read(lVisitedNode).id, lVisitedNodeView); // add render nodo figlio forward dictionary

                    var edgeConnection = lVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.leftPort
                     );
                    AddElement(lVisitedNodeView);
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
