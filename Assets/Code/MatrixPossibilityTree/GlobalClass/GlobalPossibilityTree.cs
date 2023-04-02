namespace PT.Global {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataStruct;
    using UnityEngine;

    public static class GlobalPossibilityPath {

        private static FourCTree<PossibilityPathItem> _tree = new FourCTree<PossibilityPathItem>();
		private static List<PossibilityPathItem> _goodEndsPathsDebug = new List<PossibilityPathItem>();
        private static List<GoodEndPath> _goodEndsPaths = new List<GoodEndPath>();


		public static FourCTree<PossibilityPathItem> GeneratedDebugTree {
			get { return _tree; }
		}
        public static List<PossibilityPathItem> GeneratedDebugGoodEndsPaths {
            get { return _goodEndsPathsDebug; }
        }
        public static List<GoodEndPath> GeneratedGoodEndsPaths {
            get { return _goodEndsPaths; }
        }
        



        /// <summary>
        /// Generates all the paths that start from a point of the array and arrive at an end point
        /// </summary>
        /// <param name="row">Row of matrix</param>
        /// <param name="column">Column of Matrix</param>
        public static void GeneratePathsFromMatrix() {
            _goodEndsPaths = new List<GoodEndPath>();

            int row = 3; int column = 4;


            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, column - 1);
            List<GoodEndPath> goodEndsPaths1 = GeneratePossibilitiesPathTree(row, column, startPathPosition, endPathPosition);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, column - 1);
            List<GoodEndPath> goodEndsPaths2 = GeneratePossibilitiesPathTree(row, column, startPathPosition, endPathPosition);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, column - 1);
            List<GoodEndPath> goodEndsPaths3 = GeneratePossibilitiesPathTree(row, column, startPathPosition, endPathPosition);

            
            _goodEndsPaths = goodEndsPaths1.Concat(goodEndsPaths2).ToList().Concat(goodEndsPaths3).ToList();
            _goodEndsPaths = _goodEndsPaths.OrderByDescending(item => item.score).ToList();
        }

        private static List<GoodEndPath> GeneratePossibilitiesPathTree(int row, int column, Vector2Int start, Vector2Int end) {

            _tree = new FourCTree<PossibilityPathItem>();
            _goodEndsPathsDebug = new List<PossibilityPathItem>();


            // generate starting root path
            List<Vector2Int> startingPath = new List<Vector2Int> {
                DataDeepCopy.DeepCopy(start) /* deep copy*/
            };
            _tree.InsRoot(new PossibilityPathItem(row, column, start, end, startingPath));

            
            RecursivePossibilitiesPathTreeGeneration(_tree.Root());
			return InitGoodEndPaths();
        }

        private static void RecursivePossibilitiesPathTreeGeneration(FourCTreeNode<PossibilityPathItem> root) {

            PossibilityPathItem pItem = _tree.Read(root);

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
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Forward(root));
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
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Back(root));
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
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Right(root));
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
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Left(root));
			}

            if(pItem.isDeadEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsDeadEnd();
				return;
            }
        }


		private static List<GoodEndPath> InitGoodEndPaths() {

            List<GoodEndPath> goodEndsPaths = new List<GoodEndPath>();

            Action<FourCTreeNode<PossibilityPathItem>, FourCTree<PossibilityPathItem>> onNodeVisit =
			(FourCTreeNode<PossibilityPathItem> visitedNode, FourCTree<PossibilityPathItem> tree) => {

				if(tree.Read(visitedNode).isGoodEnd()) {
					_goodEndsPathsDebug.Add(tree.Read(visitedNode));
                }
            };

            _tree.VisitTree(_tree.Root(), onNodeVisit);


			foreach(var element in _goodEndsPathsDebug) {
				goodEndsPaths.Add(
                    new GoodEndPath(
                        element.tracedPathMatrixElements,
						element.StartPathPosition(),
						element.EndPathPosition(),
						element.MatrixSize()
                    )
                );
            }

            return goodEndsPaths;
        }
    }
}
