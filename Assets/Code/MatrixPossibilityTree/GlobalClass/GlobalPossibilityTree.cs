namespace PT.Global {
    using DataStruct;
    using UnityEngine;

    public static class GlobalPossibilityTree {

        private static FourCTree<PossibilityItem> _tree = new FourCTree<PossibilityItem>();

        private static bool _isTreeGenerated = false;
        public static bool isTreeGenerated {
            get { return _isTreeGenerated; }
        }

        

        public static void GenerateTree() {
            _tree = new FourCTree<PossibilityItem>();

            _tree.InsRoot(new PossibilityItem("1"));




            _isTreeGenerated = true;
        }


        public static FourCTree<PossibilityItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
