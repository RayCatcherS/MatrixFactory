using UnityEngine;

namespace PT.DataStruct {
    public class PossibilityPathItem {
        public PossibilityPathItem(
            int matrixSize,
            Vector2Int startPathPosition,
            Vector2Int endPathPosition,
            Vector2Int[] tracedPath
        ) {

            
            _pathMatrix = new PathMatrixElement[matrixSize, matrixSize];

            _startPathPosition = startPathPosition;
            _endPathPosition = endPathPosition;

            _tracedPath = tracedPath;

            InitPathMatrix(_pathMatrix);
        }

        private PathMatrixElement[,] _pathMatrix; 
        private Vector2Int[] _tracedPath;
        public Vector2Int matrixReachedPos {
            get { return _tracedPath[_tracedPath.Length - 1]; }
        }
        private Vector2Int _startPathPosition;
        private Vector2Int _endPathPosition;

        public PathMatrixElement[,] pathMatrix {
            get { return _pathMatrix; }
        }


        public bool isForwardPosReachable() {
            bool res = false;

            return res;
        }
        public bool isBackPosReachable()  {
            bool res = false;

            return res;
        }
        public bool isRightPosReachable() {
            bool res = false;

            return res;
        }
        public bool isLeftPosReachable() {
            bool res = false;

            return res;
        }

        public bool isDeadEnd() {
            
            return isForwardPosReachable() && isBackPosReachable() && isRightPosReachable() && isLeftPosReachable();
        }

        public bool isGoodEnd() {

            return false;
        }


        public string id {
            get {
                //string _id = tracedPath.ToString();
                return "_id"; 
            }
        }

        void InitPathMatrix(PathMatrixElement[,] matrix) {

            for(int r = 0; r < matrix.GetLength(0); r++) {

                for(int c = 0; c < matrix.GetLength(1); c++) {

                    matrix[r, c] = new PathMatrixElement();
                }

            }

            for(int i = 0; i < _tracedPath.Length; i++) {

                matrix[
                    _tracedPath[i].x,
                    _tracedPath[i].y
                ].markedPos = true;
                
            }
        }
    }


    public class PathMatrixElement {
        public PathMatrixElement() {
        }
        private bool _markedPos = false;

        public bool markedPos {
            get { return _markedPos; }
            set { _markedPos = value; }
        }
    }
}
