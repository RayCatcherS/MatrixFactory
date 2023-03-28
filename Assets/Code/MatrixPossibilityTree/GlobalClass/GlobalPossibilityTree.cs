namespace PT.Global {
    using System;
    using System.Collections.Generic;
	using DataStruct;
    using UnityEngine;

    public static class GlobalPossibilityTree {

        private static FourCTree<PossibilityPathItem> _tree = new FourCTree<PossibilityPathItem>();
		private static List<PossibilityPathItem> _goodEndsPaths = new List<PossibilityPathItem>();

        private static bool _isTreeGenerated = false;
        public static bool isTreeGenerated {
            get { return _isTreeGenerated; }
        }

		public static FourCTree<PossibilityPathItem> GeneratedTree {
			get { return _tree; }
		}
        public static List<PossibilityPathItem> GoodEndsPaths {
            get { return _goodEndsPaths; }
        }


        public static void GeneratePossibilitiesPathTree(int matrixSize) {

            _tree = new FourCTree<PossibilityPathItem>();
            _goodEndsPaths = new List<PossibilityPathItem>();

            int matSize = matrixSize;

            Vector2Int endPathPosition = new Vector2Int(matSize - 1, matSize - 1);
            Vector2Int startPathPosition = new Vector2Int(0, 0);

            // generate starting root path
            List<Vector2Int> startingPath = new List<Vector2Int>();
            startingPath.Add(DataDeepCopy.DeepCopy(startPathPosition)); /* deep copy*/
            _tree.InsRoot(new PossibilityPathItem(matSize, startPathPosition, endPathPosition, startingPath));

            
            RecursivePossibilitiesPathTreeGeneration(_tree.Root());
			InitGoodEndPaths();

            _isTreeGenerated = true;
        }

        private static void RecursivePossibilitiesPathTreeGeneration(FourCTreeNode<PossibilityPathItem> root) {

            PossibilityPathItem pItem = _tree.Read(root);

			if(pItem.isGoodEnd()) {
				pItem.pathMatrix[
					pItem.matrixReachedPos.x,
					pItem.matrixReachedPos.y
				].SetAsGoodEnd();
				return;
			}

            if(pItem.isForwardPosReachable()) {
				List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPath);
				newTracedPath.Add(
					new Vector2Int(
						newTracedPath[newTracedPath.Count - 1].x - 1,
						newTracedPath[newTracedPath.Count - 1].y
					)
				);

				PossibilityPathItem newItem = new PossibilityPathItem(
					pItem.matrixSize,
					pItem.startPathPosition,
					pItem.endPathPosition,
					newTracedPath
				);

				_tree.InsForward(
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Forward(root));
			}

            if(pItem.isBackPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPath);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x + 1,
						newTracedPath[newTracedPath.Count - 1].y
					)
                );

                PossibilityPathItem newItem = new PossibilityPathItem(
                    pItem.matrixSize,
                    pItem.startPathPosition,
                    pItem.endPathPosition,
                    newTracedPath
                );

				_tree.InsBack(
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Back(root));
			}

            if(pItem.isRightPosReachable()) {
				List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPath);
				newTracedPath.Add(
					new Vector2Int(
						newTracedPath[newTracedPath.Count - 1].x,
						newTracedPath[newTracedPath.Count - 1].y + 1
					)
				);

				PossibilityPathItem newItem = new PossibilityPathItem(
					pItem.matrixSize,
					pItem.startPathPosition,
					pItem.endPathPosition,
					newTracedPath
				);

				_tree.InsRight(
					root,
					newItem
				);

                RecursivePossibilitiesPathTreeGeneration(_tree.Right(root));
			}

            if(pItem.isLeftPosReachable()) {
				List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPath);
				newTracedPath.Add(
					new Vector2Int(
						newTracedPath[newTracedPath.Count - 1].x,
						newTracedPath[newTracedPath.Count - 1].y - 1
					)
				);

				PossibilityPathItem newItem = new PossibilityPathItem(
					pItem.matrixSize,
					pItem.startPathPosition,
					pItem.endPathPosition,
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
                    pItem.matrixReachedPos.x,
                    pItem.matrixReachedPos.y
                ].SetAsDeadEnd();
				return;
            }
        }


		private static void InitGoodEndPaths() {
			Action<FourCTreeNode<PossibilityPathItem>, FourCTree<PossibilityPathItem>> onNodeVisit =
			(FourCTreeNode<PossibilityPathItem> visitedNode, FourCTree<PossibilityPathItem> tree) => {

				if(tree.Read(visitedNode).isGoodEnd()) {
					_goodEndsPaths.Add(tree.Read(visitedNode));
                }
            };

            _tree.VisitTree(_tree.Root(), onNodeVisit);
        }
    }
}
