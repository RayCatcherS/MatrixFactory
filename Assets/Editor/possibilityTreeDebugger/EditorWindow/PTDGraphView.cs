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
    using UnityEngine;

    public class PTDGraphView : GraphView {
        public PTDGraphView() {
            AddManipulators();

            AddGridBackground();

            AddStyles();
        }

        public void GenerateTreeNodesView(FourCTree<PossibilityItem> tree) {
            
            Dictionary<string, PTDNodeView> nodeViewDictionary =
            new Dictionary<string, PTDNodeView>();

            Action<FourCTreeNode<PossibilityItem>, FourCTree<PossibilityItem>> onNodeVisit =
            (FourCTreeNode<PossibilityItem> visitedNode, FourCTree<PossibilityItem> tree) => {


                PTDNodeView visitedNodeView;
                if(tree.isRoot(visitedNode)) {
                    visitedNodeView = new PTDNodeView(
                        "Node id: " + visitedNode.getItem() + "\nNode height: " + visitedNode.nodeHeight,
                        tree.isRoot(visitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );

                    nodeViewDictionary.Add(tree.Read(visitedNode).id, visitedNodeView); // add render nodo figlio forward dictionary

                    AddElement(visitedNodeView);
                } else {
                    visitedNodeView = nodeViewDictionary[tree.Read(visitedNode).id];
                }

                
                if(!tree.ForwardIsEmpty(visitedNode)) {


                    FourCTreeNode<PossibilityItem> fVisitedNode = tree.Forward(visitedNode); // forward soon node

                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodePosition(
                        visitedNodeView,
                        NodePort.forward,
                        tree.treeHeight,
                        fVisitedNode.nodeHeight
                    );


                    
                    PTDNodeView fVisitedNodeView = new PTDNodeView( // forward soon node render node
                        "Node id: " + fVisitedNode.getItem() + "\nNode height: " + fVisitedNode.nodeHeight,
                        tree.isRoot(fVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    nodeViewDictionary.Add(tree.Read(fVisitedNode).id, fVisitedNodeView); // add render nodo figlio forward dictionary

                    Edge edgeConnection = fVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.forwardPort
                     );
                    AddElement(fVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.BackIsEmpty(visitedNode)) {

                    FourCTreeNode<PossibilityItem> bVisitedNode = tree.Back(visitedNode);

                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodePosition(
                        visitedNodeView,
                        NodePort.back,
                        tree.treeHeight,
                        bVisitedNode.nodeHeight
                    );

                    
                    PTDNodeView bVisitedNodeView = new PTDNodeView(
                        "Node id: " + bVisitedNode.getItem() + "\nNode height: " + bVisitedNode.nodeHeight,
                        tree.isRoot(bVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    nodeViewDictionary.Add(tree.Read(bVisitedNode).id, bVisitedNodeView); 

                    Edge edgeConnection = bVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.backPort
                     );
                    AddElement(bVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.RightIsEmpty(visitedNode)) {

                    FourCTreeNode<PossibilityItem> rVisitedNode = tree.Right(visitedNode);

                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodePosition(
                        visitedNodeView,
                        NodePort.right,
                        tree.treeHeight,
                        rVisitedNode.nodeHeight
                    );


                    
                    PTDNodeView rVisitedNodeView = new PTDNodeView(
                        "Node id: " + rVisitedNode.getItem() + "\nNode height: " + rVisitedNode.nodeHeight,
                        tree.isRoot(rVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    nodeViewDictionary.Add(tree.Read(rVisitedNode).id, rVisitedNodeView);

                    Edge edgeConnection = rVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.rightPort
                     );
                    AddElement(rVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.LeftIsEmpty(visitedNode)) {

                    FourCTreeNode<PossibilityItem> lVisitedNode = tree.Left(visitedNode);

                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodePosition(
                        visitedNodeView,
                        NodePort.left,
                        tree.treeHeight,
                        lVisitedNode.nodeHeight
                    );


                    
                    PTDNodeView lVisitedNodeView = new PTDNodeView(
                        "Node id: " + lVisitedNode.getItem() + "\nNode height: " + lVisitedNode.nodeHeight,
                        tree.isRoot(lVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    nodeViewDictionary.Add(tree.Read(lVisitedNode).id, lVisitedNodeView); 

                    Edge edgeConnection = lVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.leftPort
                     );
                    AddElement(lVisitedNodeView);
                    AddElement(edgeConnection);
                }


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
            GenerateTreeNodesView(GlobalPossibilityTree.GetGeneratedTree());
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
