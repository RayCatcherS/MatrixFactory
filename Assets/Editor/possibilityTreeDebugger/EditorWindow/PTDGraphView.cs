using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace PT.View {

    using DebugView;
    using Global;
    using PT.DataStruct;
    using PT.DebugView.Enumerations;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PTDGraphView : GraphView {
        public PTDGraphView() {
            AddManipulators();
            initGraphView();
        }

        private Dictionary<string, PTDNodeView> _nodeTreeViewDictionary =
            new Dictionary<string, PTDNodeView>();
        private List<Edge> _nodeTreeEdges = new List<Edge>();
        private List<GeneratedLevelView> _nodesGoodEnd = new List<GeneratedLevelView>();

        private void initGraphView() {

            AddGridBackground();

            AddStyles();
        }
        
        
        public void DrawGeneratedGoodPaths(List<GeneratedLevel> scoredPaths) {

            Vector2 reachedPos = Vector2.zero;
            for(int i = 0; i < scoredPaths.Count; i++) {

                GeneratedLevelView pathView = new GeneratedLevelView(scoredPaths[i], reachedPos, i);
                AddElement(pathView);
                reachedPos = new Vector2(reachedPos.x, reachedPos.y + GeneratedLevelView.defaultSize.y);
                _nodesGoodEnd.Add(pathView);
            }
        }

        public void DrawGeneretedTreeView(FourCTree<GeneratedLevelWithMatrix> tree) {

            Action<FourCTreeNode<GeneratedLevelWithMatrix>, FourCTree<GeneratedLevelWithMatrix>> onNodeVisit =
            (FourCTreeNode<GeneratedLevelWithMatrix> visitedNode, FourCTree<GeneratedLevelWithMatrix> tree) => {


                PTDNodeView visitedNodeView;

                if(tree.isRoot(visitedNode)) {
                    visitedNodeView = new PTDNodeView(
                        tree.Read(visitedNode),
                        tree.isRoot(visitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        Vector2.zero
                    );

                    _nodeTreeViewDictionary.Add(tree.Read(visitedNode).id(), visitedNodeView); // add render nodo figlio forward dictionary

                    AddElement(visitedNodeView);
                } else {
                    visitedNodeView = _nodeTreeViewDictionary[tree.Read(visitedNode).id()];
                }

                
                if(!tree.ForwardIsEmpty(visitedNode)) {


                    FourCTreeNode<GeneratedLevelWithMatrix> fVisitedNode = tree.Forward(visitedNode); // forward soon node


                    // compute new position of child node in relation to parent node
                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodeChildPosition(
                        visitedNodeView,
                        Direction.forward,
                        tree.treeHeight,
                        fVisitedNode.nodeHeight
                    );


                    
                    PTDNodeView fVisitedNodeView = new PTDNodeView( // forward soon node render node
                        tree.Read(fVisitedNode),
                        tree.isRoot(fVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    _nodeTreeViewDictionary.Add(tree.Read(fVisitedNode).id(), fVisitedNodeView); // add render nodo figlio forward dictionary

                    Edge edgeConnection = fVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.forwardPort
                     );
                    _nodeTreeEdges.Add(edgeConnection);

                    AddElement(fVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.BackIsEmpty(visitedNode)) {

                    FourCTreeNode<GeneratedLevelWithMatrix> bVisitedNode = tree.Back(visitedNode);

                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodeChildPosition(
                        visitedNodeView,
                        Direction.back,
                        tree.treeHeight,
                        bVisitedNode.nodeHeight
                    );

                    
                    PTDNodeView bVisitedNodeView = new PTDNodeView(
                        tree.Read(bVisitedNode),
                        tree.isRoot(bVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    _nodeTreeViewDictionary.Add(tree.Read(bVisitedNode).id(), bVisitedNodeView); 

                    Edge edgeConnection = bVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.backPort
                    );
                    _nodeTreeEdges.Add(edgeConnection);
                    
                    AddElement(bVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.RightIsEmpty(visitedNode)) {

                    FourCTreeNode<GeneratedLevelWithMatrix> rVisitedNode = tree.Right(visitedNode);

                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodeChildPosition(
                        visitedNodeView,
                        Direction.right,
                        tree.treeHeight,
                        rVisitedNode.nodeHeight
                    );


                    
                    PTDNodeView rVisitedNodeView = new PTDNodeView(
                        tree.Read(rVisitedNode),
                        tree.isRoot(rVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    _nodeTreeViewDictionary.Add(tree.Read(rVisitedNode).id(), rVisitedNodeView);

                    Edge edgeConnection = rVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.rightPort
                    );
                    _nodeTreeEdges.Add(edgeConnection);

                    AddElement(rVisitedNodeView);
                    AddElement(edgeConnection);
                }
                if(!tree.LeftIsEmpty(visitedNode)) {

                    FourCTreeNode<GeneratedLevelWithMatrix> lVisitedNode = tree.Left(visitedNode);


                    Vector2 nodeSpawnPos = PTDNodeView.calculateRelativeNodeChildPosition(
                        visitedNodeView,
                        Direction.left,
                        tree.treeHeight,
                        lVisitedNode.nodeHeight
                    );


                    
                    PTDNodeView lVisitedNodeView = new PTDNodeView(
                        tree.Read(lVisitedNode),
                        tree.isRoot(lVisitedNode) ? PTNodeType.RootNode : PTNodeType.InternalNode,
                        nodeSpawnPos
                    );
                    _nodeTreeViewDictionary.Add(tree.Read(lVisitedNode).id(), lVisitedNodeView); 

                    Edge edgeConnection = lVisitedNodeView.parentPort.ConnectTo(
                        visitedNodeView.leftPort
                    );
                    _nodeTreeEdges.Add(edgeConnection);

                    AddElement(lVisitedNodeView);
                    AddElement(edgeConnection);
                }


            };


            tree.VisitTree(tree.Root(), onNodeVisit);
        }

        private void AddManipulators() {
            SetupZoom(0.05f, 10/*ContentZoomer.DefaultMaxScale*/);

			this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            

            this.AddManipulator(DrawGeneratedGoodPathsContextualMenu());
            this.AddManipulator(DrawGeneratedPossibilityTreeContextualMenu());
            this.AddManipulator(GeneratePossibilityTreeContextualMenu());
            this.AddManipulator(ClearGraphInterfaceContextualMenu());
        }

        private IManipulator DrawGeneratedPossibilityTreeContextualMenu() {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Draw Possibility Tree", actionEvent => DrawGeneretedTreeView())
            );

            return contextualMenuManipulator;
        }

        private IManipulator DrawGeneratedGoodPathsContextualMenu() {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    "Draw generated good paths",
                    actionEvent => DrawGeneratedGoodPaths()
                    )
            );

            return contextualMenuManipulator;
        }

        private IManipulator GeneratePossibilityTreeContextualMenu() {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Generate Possibility Tree Example (2x2)", actionEvent => GeneratePossibilityTree())
            );

            return contextualMenuManipulator;
        }

        private IManipulator ClearGraphInterfaceContextualMenu() {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
               menuEvent => menuEvent.menu.AppendAction("Clear Interface", actionEvent => ClearInterface())
           );
            return contextualMenuManipulator;
        }

        private void DrawGeneretedTreeView() {
            ClearInterface();
            FourCTree<GeneratedLevelWithMatrix> _tree = GlobalPossibilityPath.GeneratedDebugTree;
            DrawGeneretedTreeView(_tree);
        }
        private void DrawGeneratedGoodPaths() {
            ClearInterface();
            List<GeneratedLevel> _paths = GlobalPossibilityPath.GetChapterLevels(GameSaveManager.CurrentReachedLevel.Chapter);
            DrawGeneratedGoodPaths(_paths);
        }
        

        private void GeneratePossibilityTree() {
            GlobalPossibilityPath.GenerateChaptersPaths(true, true);
        }

        private void ClearInterface() {

            foreach(var element in _nodeTreeViewDictionary) {
                this.RemoveElement(element.Value);
            }
            foreach(var element in _nodeTreeEdges) {
                this.RemoveElement(element);
            }
            foreach(var element in _nodesGoodEnd) {
                this.RemoveElement(element);
            }
            _nodesGoodEnd = new List<GeneratedLevelView>();
            _nodeTreeViewDictionary = new Dictionary<string, PTDNodeView>();
            _nodeTreeEdges = new List<Edge>();
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
