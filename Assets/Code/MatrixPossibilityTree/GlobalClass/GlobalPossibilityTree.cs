namespace PT.Global {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataStruct;
    using Unity.VisualScripting.Antlr3.Runtime.Tree;
    using UnityEngine;

    public static class GlobalPossibilityPath {

        private static FourCTree<PossibilityPathItem> _tree = new FourCTree<PossibilityPathItem>();
		private static List<PossibilityPathItem> _goodEndsDebugPathsItems = new List<PossibilityPathItem>();
        private static List<GoodEndPath> _goodEndsPaths = new List<GoodEndPath>();


		public static FourCTree<PossibilityPathItem> GeneratedDebugTree {
			get { return _tree; }
		}
        public static List<PossibilityPathItem> GeneratedDebugGoodEndsPaths {
            get { return _goodEndsDebugPathsItems; }
        }
        public static List<GoodEndPath> GeneratedGoodEndsPaths {
            get { return _goodEndsPaths; }
        }
        



        /// <summary>
        /// Generates all the paths that start from a point of the array and arrive at an end point
        /// </summary>
        /// <param name="row">Row of matrix</param>
        /// <param name="column">Column of Matrix</param>
        public static void GeneratePaths(int rows, int columns, bool debugStatistic = false, bool memorizeAllPossibilityPathTree = false) {

            double timer = 0;
            if(debugStatistic) {
                Debug.Log("Timer started");
                timer = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
            

            _goodEndsPaths = new List<GoodEndPath>();

            int _rows = rows; int _columns = columns;


            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, _columns - 1);
            List<GoodEndPath> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, _columns - 1);
            List<GoodEndPath> goodEndsPaths2 = new List<GoodEndPath>();
            //goodEndsPaths2 = GeneratePossibilitiesPathTree(row, column, startPathPosition, endPathPosition);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, _columns - 1);
            List<GoodEndPath> goodEndsPaths3 = new List<GoodEndPath>();
            //goodEndsPaths3 = GeneratePossibilitiesPathTree(row, column, startPathPosition, endPathPosition);*/

            
            _goodEndsPaths = goodEndsPaths1.Concat(goodEndsPaths2).ToList().Concat(goodEndsPaths3).ToList();
            _goodEndsPaths = _goodEndsPaths.OrderByDescending(item => item.score).ToList();

            if(debugStatistic) {
                Debug.Log("Timer ended in: " + (System.DateTime.Now.TimeOfDay.TotalMilliseconds - timer));
            }
                
        }

        

        private static List<GoodEndPath> GeneratePossibilitiesPathTree(int row, int column, Vector2Int start, Vector2Int end, bool memorizeAllPossibilityPathTree = false) {

            _tree = new FourCTree<PossibilityPathItem>();
            _goodEndsDebugPathsItems = new List<PossibilityPathItem>();


            // generate starting root path
            List<Vector2Int> startingPath = new List<Vector2Int> {
                DataDeepCopy.DeepCopy(start) /* deep copy*/
            };
            PossibilityPathItem rootItem = new PossibilityPathItem(row, column, start, end, startingPath);
            _tree.InsRoot(rootItem);


            if(memorizeAllPossibilityPathTree) {
                RecursivePossibilitiesPathTreeGeneration(_tree.Root());

                return GetGoodEndPathsFromPossibilityDebugTree();
            } else {

                List<GoodEndPath> goodEndPaths = new List<GoodEndPath>();
                RecursivePossibilitiesPathGeneration(rootItem, goodEndPaths);
                return goodEndPaths;
            }
            
            
            
        }

        private static void RecursivePossibilitiesPathGeneration(PossibilityPathItem item, List<GoodEndPath> _goodPaths) {

            PossibilityPathItem pItem = item;

            if(pItem.isGoodEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsGoodEnd();

                _goodPaths.Add(
                    new GoodEndPath(
                        pItem.tracedPathMatrixElements,
                        pItem.StartPathPosition(),
                        pItem.EndPathPosition(),
                        pItem.MatrixSize()
                    )
                );    


                return;
            }

            if(pItem.isForwardPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x - 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isBackPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x + 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isRightPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y + 1
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isLeftPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y - 1
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isDeadEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsDeadEnd();
                return;
            }
        }

        private static void RecursivePossibilitiesPathTreeGeneration(FourCTreeNode<PossibilityPathItem> node) {

            PossibilityPathItem pItem = _tree.Read(node);

            if(pItem.isGoodEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsGoodEnd();
                return;
            }

            if(pItem.isForwardPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x - 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsForward(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Forward(node));
            }

            if(pItem.isBackPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x + 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsBack(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Back(node));
            }

            if(pItem.isRightPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y + 1
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsRight(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Right(node));
            }

            if(pItem.isLeftPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y - 1
                    )
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsLeft(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Left(node));
            }

            if(pItem.isDeadEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsDeadEnd();
                return;
            }
        }

        private static List<GoodEndPath> GetGoodEndPathsFromPossibilityDebugTree() {

            List<GoodEndPath> goodEndsPaths = new List<GoodEndPath>();

            Action<FourCTreeNode<PossibilityPathItem>, FourCTree<PossibilityPathItem>> onNodeVisit =
            (FourCTreeNode<PossibilityPathItem> visitedNode, FourCTree<PossibilityPathItem> tree) => {

                if(tree.Read(visitedNode).isGoodEnd()) {
                    _goodEndsDebugPathsItems.Add(tree.Read(visitedNode));

                    goodEndsPaths.Add(
                        new GoodEndPath(
                            tree.Read(visitedNode).tracedPathMatrixElements,
                            tree.Read(visitedNode).StartPathPosition(),
                            tree.Read(visitedNode).EndPathPosition(),
                            tree.Read(visitedNode).MatrixSize()
                        )
                    );
                }
            };

            _tree.VisitTree(_tree.Root(), onNodeVisit);


            /*foreach(var element in _goodEndsPathsDebug) {
				goodEndsPaths.Add(
                    new GoodEndPath(
                        element.tracedPathMatrixElements,
						element.StartPathPosition(),
						element.EndPathPosition(),
						element.MatrixSize()
                    )
                );
            }*/

            return goodEndsPaths;
        }
    }
}
