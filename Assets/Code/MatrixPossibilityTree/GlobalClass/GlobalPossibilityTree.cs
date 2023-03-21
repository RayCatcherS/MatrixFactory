namespace PT.Global {
    using DataStruct;

    public static class GlobalPossibilityTree {

        private static FourCTree<PossibilityItem> _tree;

        private static bool _isTreeGenerated = false;
        public static bool isTreeGenerated {
            get { return _isTreeGenerated; }
        }

        

        public static void GenerateTree() {
            _tree = new FourCTree<PossibilityItem>();

            _tree.InsRoot(new PossibilityItem("1"));
            _tree.InsLeft(_tree.Root(), new PossibilityItem("2"));
            _tree.InsLeft(_tree.Left(_tree.Root()), new PossibilityItem("4"));
            _tree.InsRight(_tree.Left(_tree.Root()), new PossibilityItem("5"));
            _tree.InsLeft(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("8"));


            _tree.InsRight(_tree.Root(), new PossibilityItem("3"));


            _isTreeGenerated = true;
        }


        public static FourCTree<PossibilityItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
