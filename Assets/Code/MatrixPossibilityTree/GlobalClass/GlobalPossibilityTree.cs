namespace PT.Global {
	using System.Collections.Generic;
	using DataStruct;
    using UnityEngine;

    public static class GlobalPossibilityTree {

        private static FourCTree<PossibilityPathItem> _tree = new FourCTree<PossibilityPathItem>();

        private static bool _isTreeGenerated = false;
        public static bool isTreeGenerated {
            get { return _isTreeGenerated; }
        }

        

        public static void GeneratePossibilitiesPathTree(int matrixSize) {

            _tree = new FourCTree<PossibilityPathItem>();


            Vector2Int endPathPosition = new Vector2Int(2, 2);
            Vector2Int startPathPosition = new Vector2Int(0, 0);

            // generate starting root path
            List<Vector2Int> startingPath = new List<Vector2Int>();
            startingPath.Add(DataDeepCopy.DeepCopy(startPathPosition)); /* deep copy*/
            _tree.InsRoot(new PossibilityPathItem(3, startPathPosition, endPathPosition, startingPath));

            

            recursivePossibilitiesPathTreeGeneration(_tree.Root());


            _isTreeGenerated = true;
        }

        private static void recursivePossibilitiesPathTreeGeneration(FourCTreeNode<PossibilityPathItem> root) {

            PossibilityPathItem pItem = _tree.Read(root);

			if(pItem.isGoodEnd()) {
				pItem.pathMatrix[
					pItem.matrixReachedPos.x,
					pItem.matrixReachedPos.y
				].setAsGoodEnd();
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

				recursivePossibilitiesPathTreeGeneration(_tree.Forward(root));
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

                recursivePossibilitiesPathTreeGeneration(_tree.Back(root));
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

				recursivePossibilitiesPathTreeGeneration(_tree.Right(root));
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

				recursivePossibilitiesPathTreeGeneration(_tree.Left(root));
			}

            if(pItem.isDeadEnd()) {
				Debug.Log("dead end");
                pItem.pathMatrix[
                    pItem.matrixReachedPos.x,
                    pItem.matrixReachedPos.y
                ].setAsDeadEnd();
				return;
            }
        }


        public static FourCTree<PossibilityPathItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
