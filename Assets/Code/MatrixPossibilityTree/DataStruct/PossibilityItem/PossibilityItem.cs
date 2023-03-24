using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace PT.DataStruct {
    public class PossibilityItem {
        public PossibilityItem(string id) {
            _id = id;
        }
        private string _id;
        private PathMatrix _pathMatrix;

        bool isMatrixForwardPosReachable() {
            bool res = false;

            return res;
        }
        bool isMatrixBackPosReachable()
        {
            bool res = false;

            return res;
        }
        bool isMatrixRightPosReachable()
        {
            bool res = false;

            return res;
        }
        bool isMatrixLeftPosReachable()
        {
            bool res = false;

            return res;
        }



        public string id {
            get { return _id; }
        }

        public override string ToString() {
            return _id;
        }
    }

    public class PathMatrix {
        PathMatrixElement[][] matrix;
        Vector2Int reachedPos;
    }

    public class PathMatrixElement {
        private bool _markedPos = false;

        public bool markedPos {
            get { return _markedPos; }
            set { _markedPos = value; }
        }
    }
}
