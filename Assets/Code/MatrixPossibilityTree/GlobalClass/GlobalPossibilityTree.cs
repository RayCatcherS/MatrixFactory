namespace PT.Global {
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
            Vector2Int[] startingPath = new Vector2Int[] { DataDeepCopy.DeepCopy(startPathPosition) /* deep copy*/ };
            _tree.InsRoot(new PossibilityPathItem(2, startPathPosition, endPathPosition, startingPath));

            

            recursivePossibilitiesPathTreeGeneration(_tree.Root());


            _isTreeGenerated = true;
        }

        private static void recursivePossibilitiesPathTreeGeneration(FourCTreeNode<PossibilityPathItem> root) {

            PossibilityPathItem pItem = _tree.Read(root);


            if(pItem.isForwardPosReachable()) {

            }

            if(pItem.isBackPosReachable()) {

            }

            if(pItem.isRightPosReachable()) {

            }

            if(pItem.isLeftPosReachable()) {

            }

            if(pItem.isDeadEnd()) {

            }
        }


        public static FourCTree<PossibilityPathItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
