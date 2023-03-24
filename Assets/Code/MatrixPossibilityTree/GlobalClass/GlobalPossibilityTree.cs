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

            _tree.InsForward(_tree.Root(), new PossibilityItem("5"));
            _tree.InsForward(_tree.Forward(_tree.Root()), new PossibilityItem("10"));
            _tree.InsBack(_tree.Forward(_tree.Root()), new PossibilityItem("11"));
            _tree.InsRight(_tree.Forward(_tree.Root()), new PossibilityItem("12"));
            _tree.InsLeft(_tree.Forward(_tree.Root()), new PossibilityItem("13"));


            _tree.InsBack(_tree.Root(), new PossibilityItem("2"));
            _tree.InsForward(_tree.Back(_tree.Root()), new PossibilityItem("20"));
            _tree.InsBack(_tree.Back(_tree.Root()), new PossibilityItem("21"));
            _tree.InsRight(_tree.Back(_tree.Root()), new PossibilityItem("22"));
            _tree.InsLeft(_tree.Back(_tree.Root()), new PossibilityItem("23"));



            _tree.InsRight(_tree.Root(), new PossibilityItem("3"));
            _tree.InsForward(_tree.Right(_tree.Root()), new PossibilityItem("31"));
            _tree.InsBack(_tree.Right(_tree.Root()), new PossibilityItem("32"));
            _tree.InsRight(_tree.Right(_tree.Root()), new PossibilityItem("33"));
            _tree.InsLeft(_tree.Right(_tree.Root()), new PossibilityItem("34"));



            _tree.InsLeft(_tree.Root(), new PossibilityItem("4"));




            _isTreeGenerated = true;
        }


        public static FourCTree<PossibilityItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
