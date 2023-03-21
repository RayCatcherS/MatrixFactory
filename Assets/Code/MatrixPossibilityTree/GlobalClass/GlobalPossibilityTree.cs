


namespace PT.Global {
    using DataStruct;
    public static class GlobalPossibilityTree {

        private static bool _isTreeGenerated = false;
        private static FourCTree<string> _tree;
        public static void GenerateTree() {
            _tree = new FourCTree<string>();

            _tree.InsRoot("prova");
            _tree.Forward(_tree.Root(), "figlio forward");
            _tree.Right(_tree.Root(), "figlio right");


            _isTreeGenerated = true;
        }

        public static FourCTree<string> GetGeneratedTree() {
            return _tree;
        }
    }
}
