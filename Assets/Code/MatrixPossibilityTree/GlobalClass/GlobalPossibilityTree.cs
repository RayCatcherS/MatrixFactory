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
            _tree.InsForward(_tree.Left(_tree.Root()), new PossibilityItem("11"));
            _tree.InsForward(_tree.Forward(_tree.Left(_tree.Root())), new PossibilityItem("50"));
            _tree.InsBack(_tree.Forward(_tree.Left(_tree.Root())), new PossibilityItem("51"));
            _tree.InsRight(_tree.Forward(_tree.Left(_tree.Root())), new PossibilityItem("52"));
            _tree.InsLeft(_tree.Forward(_tree.Left(_tree.Root())), new PossibilityItem("53"));



            _tree.InsBack(_tree.Left(_tree.Root()), new PossibilityItem("12"));
            _tree.InsForward(_tree.Back(_tree.Left(_tree.Root())), new PossibilityItem("60"));
            _tree.InsBack(_tree.Back(_tree.Left(_tree.Root())), new PossibilityItem("61"));
            _tree.InsRight(_tree.Back(_tree.Left(_tree.Root())), new PossibilityItem("62"));
            _tree.InsLeft(_tree.Back(_tree.Left(_tree.Root())), new PossibilityItem("63"));



            _tree.InsRight(_tree.Left(_tree.Root()), new PossibilityItem("13"));
            _tree.InsForward(_tree.Right(_tree.Left(_tree.Root())), new PossibilityItem("70"));
            _tree.InsBack(_tree.Right(_tree.Left(_tree.Root())), new PossibilityItem("71"));
            _tree.InsRight(_tree.Right(_tree.Left(_tree.Root())), new PossibilityItem("72"));
            _tree.InsLeft(_tree.Right(_tree.Left(_tree.Root())), new PossibilityItem("73"));



            _tree.InsLeft(_tree.Left(_tree.Root()), new PossibilityItem("14"));
            _tree.InsForward(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("80"));
            _tree.InsBack(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("81"));


            _tree.InsRight(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("82"));
            _tree.InsForward(_tree.Right(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("401"));
            _tree.InsBack(_tree.Right(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("402"));
            _tree.InsRight(_tree.Right(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("403"));
            _tree.InsLeft(_tree.Right(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("404"));



            _tree.InsLeft(_tree.Left(_tree.Left(_tree.Root())), new PossibilityItem("83"));
            _tree.InsForward(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("501"));
            _tree.InsBack(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("502"));
            _tree.InsRight(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("503"));
            _tree.InsLeft(_tree.Left(_tree.Left(_tree.Left(_tree.Root()))), new PossibilityItem("504"));




            _tree.InsRight(_tree.Root(), new PossibilityItem("3"));
            _tree.InsForward(_tree.Right(_tree.Root()), new PossibilityItem("21"));
            _tree.InsForward(_tree.Forward(_tree.Right(_tree.Root())), new PossibilityItem("90"));
            _tree.InsBack(_tree.Forward(_tree.Right(_tree.Root())), new PossibilityItem("91"));
            _tree.InsRight(_tree.Forward(_tree.Right(_tree.Root())), new PossibilityItem("92"));
            _tree.InsLeft(_tree.Forward(_tree.Right(_tree.Root())), new PossibilityItem("93"));




            _tree.InsBack(_tree.Right(_tree.Root()), new PossibilityItem("22"));
            _tree.InsForward(_tree.Back(_tree.Right(_tree.Root())), new PossibilityItem("100"));
            _tree.InsBack(_tree.Back(_tree.Right(_tree.Root())), new PossibilityItem("101"));
            _tree.InsRight(_tree.Back(_tree.Right(_tree.Root())), new PossibilityItem("102"));
            _tree.InsLeft(_tree.Back(_tree.Right(_tree.Root())), new PossibilityItem("103"));



            _tree.InsRight(_tree.Right(_tree.Root()), new PossibilityItem("23"));
            _tree.InsForward(_tree.Right(_tree.Right(_tree.Root())), new PossibilityItem("110"));
            _tree.InsBack(_tree.Right(_tree.Right(_tree.Root())), new PossibilityItem("111"));
            _tree.InsRight(_tree.Right(_tree.Right(_tree.Root())), new PossibilityItem("112"));
            _tree.InsLeft(_tree.Right(_tree.Right(_tree.Root())), new PossibilityItem("113"));



            _tree.InsLeft(_tree.Right(_tree.Root()), new PossibilityItem("24"));
            _tree.InsForward(_tree.Left(_tree.Right(_tree.Root())), new PossibilityItem("120"));
            _tree.InsBack(_tree.Left(_tree.Right(_tree.Root())), new PossibilityItem("121"));
            _tree.InsRight(_tree.Left(_tree.Right(_tree.Root())), new PossibilityItem("122"));
            _tree.InsLeft(_tree.Left(_tree.Right(_tree.Root())), new PossibilityItem("123"));




            _tree.InsForward(_tree.Root(), new PossibilityItem("6"));
            _tree.InsForward(_tree.Forward(_tree.Root()), new PossibilityItem("31"));
            _tree.InsForward(_tree.Forward(_tree.Forward(_tree.Root())), new PossibilityItem("130"));
            _tree.InsBack(_tree.Forward(_tree.Forward(_tree.Root())), new PossibilityItem("131"));
            _tree.InsRight(_tree.Forward(_tree.Forward(_tree.Root())), new PossibilityItem("132"));
            _tree.InsLeft(_tree.Forward(_tree.Forward(_tree.Root())), new PossibilityItem("133"));



            _tree.InsBack(_tree.Forward(_tree.Root()), new PossibilityItem("32"));
            _tree.InsForward(_tree.Back(_tree.Forward(_tree.Root())), new PossibilityItem("140"));
            _tree.InsBack(_tree.Back(_tree.Forward(_tree.Root())), new PossibilityItem("141"));
            _tree.InsRight(_tree.Back(_tree.Forward(_tree.Root())), new PossibilityItem("142"));
            _tree.InsLeft(_tree.Back(_tree.Forward(_tree.Root())), new PossibilityItem("143"));



            _tree.InsRight(_tree.Forward(_tree.Root()), new PossibilityItem("33"));
            _tree.InsForward(_tree.Right(_tree.Forward(_tree.Root())), new PossibilityItem("150"));
            _tree.InsBack(_tree.Right(_tree.Forward(_tree.Root())), new PossibilityItem("151"));
            _tree.InsRight(_tree.Right(_tree.Forward(_tree.Root())), new PossibilityItem("152"));
            _tree.InsLeft(_tree.Right(_tree.Forward(_tree.Root())), new PossibilityItem("153"));



            _tree.InsLeft(_tree.Forward(_tree.Root()), new PossibilityItem("34"));
            _tree.InsForward(_tree.Left(_tree.Forward(_tree.Root())), new PossibilityItem("160"));
            _tree.InsBack(_tree.Left(_tree.Forward(_tree.Root())), new PossibilityItem("161"));
            _tree.InsRight(_tree.Left(_tree.Forward(_tree.Root())), new PossibilityItem("162"));
            _tree.InsLeft(_tree.Left(_tree.Forward(_tree.Root())), new PossibilityItem("163"));



            _tree.InsBack(_tree.Root(), new PossibilityItem("7"));
            _tree.InsForward(_tree.Back(_tree.Root()), new PossibilityItem("41"));
            _tree.InsForward(_tree.Forward(_tree.Back(_tree.Root())), new PossibilityItem("170"));
            _tree.InsBack(_tree.Forward(_tree.Back(_tree.Root())), new PossibilityItem("171"));
            _tree.InsRight(_tree.Forward(_tree.Back(_tree.Root())), new PossibilityItem("172"));
            _tree.InsLeft(_tree.Forward(_tree.Back(_tree.Root())), new PossibilityItem("173"));



            _tree.InsBack(_tree.Back(_tree.Root()), new PossibilityItem("42"));
            _tree.InsForward(_tree.Back(_tree.Back(_tree.Root())), new PossibilityItem("180"));
            _tree.InsBack(_tree.Back(_tree.Back(_tree.Root())), new PossibilityItem("181"));
            _tree.InsRight(_tree.Back(_tree.Back(_tree.Root())), new PossibilityItem("182"));
            _tree.InsLeft(_tree.Back(_tree.Back(_tree.Root())), new PossibilityItem("183"));



            _tree.InsRight(_tree.Back(_tree.Root()), new PossibilityItem("43"));
            _tree.InsForward(_tree.Right(_tree.Back(_tree.Root())), new PossibilityItem("190"));
            _tree.InsBack(_tree.Right(_tree.Back(_tree.Root())), new PossibilityItem("191"));
            _tree.InsRight(_tree.Right(_tree.Back(_tree.Root())), new PossibilityItem("192"));
            _tree.InsLeft(_tree.Right(_tree.Back(_tree.Root())), new PossibilityItem("193"));



            _tree.InsLeft(_tree.Back(_tree.Root()), new PossibilityItem("44"));
            _tree.InsForward(_tree.Left(_tree.Back(_tree.Root())), new PossibilityItem("200"));
            _tree.InsBack(_tree.Left(_tree.Back(_tree.Root())), new PossibilityItem("201"));
            _tree.InsRight(_tree.Left(_tree.Back(_tree.Root())), new PossibilityItem("202"));
            _tree.InsLeft(_tree.Left(_tree.Back(_tree.Root())), new PossibilityItem("203"));




            _isTreeGenerated = true;
        }


        public static FourCTree<PossibilityItem> GetGeneratedTree() {
            return _tree;
        }
    }
}
