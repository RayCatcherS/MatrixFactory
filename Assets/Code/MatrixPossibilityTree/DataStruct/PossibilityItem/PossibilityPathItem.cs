using System.Collections.Generic;
using UnityEngine;

namespace PT.DataStruct {
    public class PossibilityPathItem {
		public PossibilityPathItem (
			int matrixSize,
			Vector2Int startPathPosition,
			Vector2Int endPathPosition,
			List<Vector2Int> tracedPath
		) {



			_pathMatrix = new PathMatrixElement[matrixSize, matrixSize];

			_startPathPosition = startPathPosition;
			_endPathPosition = endPathPosition;

			_tracedPath = tracedPath;

			InitPathMatrix (_pathMatrix);
		}

		private PathMatrixElement [,] _pathMatrix;
		private List<Vector2Int> _tracedPath;
		private Vector2Int _startPathPosition;
		private Vector2Int _endPathPosition;
	
		public int matrixSize {
				get { return _pathMatrix.GetLength(0); }
		}
		public List<Vector2Int> tracedPath {
				get { return _tracedPath; }
		}
		public Vector2Int startPathPosition {
				get { return _startPathPosition; }
		}
		public Vector2Int endPathPosition {
				get { return _endPathPosition; }
		}
		public Vector2Int matrixReachedPos {
			get { return _tracedPath [_tracedPath.Count - 1]; }
		}
		public PathMatrixElement [,] pathMatrix {
			get { return _pathMatrix; }

		}


		public bool isForwardPosReachable() {

			Vector2Int newMove = new Vector2Int(matrixReachedPos.x - 1, matrixReachedPos.y);
			bool correct = isCorrectMove(newMove);

			return correct;
		}
		public bool isBackPosReachable() {

			Vector2Int newMove = new Vector2Int(matrixReachedPos.x + 1, matrixReachedPos.y);
			bool correct = isCorrectMove(newMove);

			return correct;

		}
		public bool isRightPosReachable() {
			Vector2Int newMove = new Vector2Int(matrixReachedPos.x, matrixReachedPos.y + 1);
			bool correct = isCorrectMove(newMove);

			return correct;
		}
		public bool isLeftPosReachable() {
			Vector2Int newMove = new Vector2Int(matrixReachedPos.x, matrixReachedPos.y - 1);
			bool correct = isCorrectMove(newMove);

			return correct;
		}

		public bool isDeadEnd() {

			return !isForwardPosReachable() && !isBackPosReachable () && !isRightPosReachable () && !isLeftPosReachable ();
		}

		public bool isGoodEnd() {

			bool res = false;
			if(matrixReachedPos.x == endPathPosition.x && matrixReachedPos.y == endPathPosition.y) {
				res = true;
			}


			return res;
		}

		 
		private bool isCorrectMove(Vector2Int move) {
			bool res = false;

			if(move.x < 0 || move.y < 0 || move.x > _pathMatrix.GetLength(0)-1 || move.y > _pathMatrix.GetLength(1) - 1) {
			
				res = false;
		    
			} else {

				if (_pathMatrix[move.x, move.y].tracedPos) {
					res = false;
				} else {
					res = true;
				}
			}
			return res;
		
		}


		public string id {
			get {
				string idValue = "";
				for (int i = 0; i < tracedPath.Count; i++) {
					idValue = idValue + tracedPath[i] + ";";
				}
				return idValue;
			}
		}

		void InitPathMatrix (PathMatrixElement [,] matrix) {

			for (int r = 0; r < matrix.GetLength (0); r++) {

				for (int c = 0; c < matrix.GetLength(1); c++) {

					matrix [r, c] = new PathMatrixElement(new Vector2Int (r, c));
				}

			}

			for (int i = 0; i < _tracedPath.Count; i++) {

				Vector2Int pos = _tracedPath[i];
				PathMatrixElement element = matrix[
                    pos.x,
                    pos.y
                ];

				element.SetAsTracedPos();


                // set trace direction
                if(_tracedPath[i] != matrixReachedPos) {
					Vector2Int direction = _tracedPath[i + 1] - _tracedPath[i];

					if(direction == new Vector2Int(-1, 0)) {
                        element.SetTracedMoveDirection(Direction.forward);

                    } else if(direction == new Vector2Int(1, 0)) {
                        element.SetTracedMoveDirection(Direction.back);

                    } else if(direction == new Vector2Int(0, 1)) {
                        element.SetTracedMoveDirection(Direction.right);

                    } else if(direction == new Vector2Int(0, -1)) {
                        element.SetTracedMoveDirection(Direction.left);

                    }

                } else {
                    element.SetTracedMoveDirection(Direction.stay);
                }

                
            }
		}
	}


	public class PathMatrixElement {
		public PathMatrixElement(Vector2Int pos) {
			_pos = pos;
		}
		private Vector2Int _pos;
		private Direction _tracedMoveDirection;

        private bool _tracedPos = false;
		private bool _deadEnd = false;
		private bool _goodEnd = false;
		public bool deadEnd {
			get { return _deadEnd; }
		}
		public bool goodEnd {
			get { return _goodEnd; }
		}

		public bool tracedPos {
			get { return _tracedPos; }
		}

		public Vector2Int pos {
			get { return _pos; }
		}

		public Direction tracedMoveDirection {
			get { return _tracedMoveDirection; }
		}

        public void SetAsDeadEnd() {
			_deadEnd = true;
		}
		public void SetAsGoodEnd() {
			_goodEnd = true;
		}
		public void SetAsTracedPos() {
            _tracedPos = true;
		}

		public void SetTracedMoveDirection(Direction direction) {
			_tracedMoveDirection = direction;
		}
	}
}
