namespace PT.Global {
    using DataStruct;
    using UnityEngine;

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
            _tree.InsRight(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("60"));




            _tree.InsRight(_tree.Left(_tree.Root()), new PossibilityItem("5"));
            _tree.InsForward(_tree.Left(_tree.Root()), new PossibilityItem("11"));
            _tree.InsBack(_tree.Left(_tree.Root()), new PossibilityItem("13"));
            _tree.InsLeft(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("8"));
            _tree.InsBack(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("22"));
            _tree.InsForward(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("23"));
            _tree.InsRight(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("24"));
            _tree.InsLeft(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("25"));

            _tree.InsForward(_tree.Back(_tree.Left(_tree.Left(_tree.Left(_tree.Root())))), new PossibilityItem("50"));
            _tree.InsBack(_tree.Back(_tree.Left(_tree.Left(_tree.Left(_tree.Root())))), new PossibilityItem("51"));
            _tree.InsRight(_tree.Back(_tree.Left(_tree.Left(_tree.Left(_tree.Root())))), new PossibilityItem("52"));
            _tree.InsLeft(_tree.Back(_tree.Left(_tree.Left(_tree.Left(_tree.Root())))), new PossibilityItem("53"));


            _tree.InsRight(_tree.Root(), new PossibilityItem("3"));

            _tree.InsForward(_tree.Root(), new PossibilityItem("6"));

            _tree.InsBack(_tree.Root(), new PossibilityItem("7"));

            _isTreeGenerated = true;
        }


        public static FourCTree<PossibilityItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
