using System.Collections.Generic;
using UnityEngine;

namespace PT.DataStruct {
    public class PossibilityPathItem : Path {
        public PossibilityPathItem (
			int row,
			int column,
			Vector2Int startPathPosition,
			Vector2Int endPathPosition,
			List<Vector2Int> tracedPathPositions
		) {

			_pathMatrix = new PossibilityMatrixPathElement[row, column];

			_startPathPosition = new Vector2Int(startPathPosition.x, startPathPosition.y);
			_endPathPosition = new Vector2Int(endPathPosition.x, endPathPosition.y);

			_tracedPathPositions = tracedPathPositions;

			InitPathMatrix();
		}

		private PossibilityMatrixPathElement [,] _pathMatrix;
		private List<Vector2Int> _tracedPathPositions;
		private List<PossibilityMatrixPathElement> _tracedPathMatrixElements = new List<PossibilityMatrixPathElement>();

        private Vector2Int _startPathPosition;
		private Vector2Int _endPathPosition;

        public int row {
				get { return _pathMatrix.GetLength(0); }
		}
        public int column {
				get { return _pathMatrix.GetLength(1); }
		}

		public Vector2Int MatrixSize() {
            return new Vector2Int(row, column);
        }

        public List<Vector2Int> tracedPathPositions {
				get { return _tracedPathPositions; }
		}
        public List<PossibilityMatrixPathElement> tracedPathMatrixElements {
            get { return _tracedPathMatrixElements; }
        }
        public Vector2Int StartPathPosition() {
            return _startPathPosition;
        }
		public Vector2Int EndPathPosition() {
            return _endPathPosition;
        }
		public Vector2Int LastPos() {
            return _tracedPathPositions[_tracedPathPositions.Count - 1];
        }
		
		/// <summary>
		/// Get the matrix with the path
		/// </summary>
		public PossibilityMatrixPathElement [,] pathMatrix {
			get { return _pathMatrix; }

		}


		public bool isForwardPosReachable() {

			Vector2Int newMove = new Vector2Int(LastPos().x - 1, LastPos().y);
			bool correct = isCorrectMove(newMove);

			return correct;
		}
		public bool isBackPosReachable() {

			Vector2Int newMove = new Vector2Int(LastPos().x + 1, LastPos().y);
			bool correct = isCorrectMove(newMove);

			return correct;

		}
		public bool isRightPosReachable() {
			Vector2Int newMove = new Vector2Int(LastPos().x, LastPos().y + 1);
			bool correct = isCorrectMove(newMove);

			return correct;
		}
		public bool isLeftPosReachable() {
			Vector2Int newMove = new Vector2Int(LastPos().x, LastPos().y - 1);
			bool correct = isCorrectMove(newMove);

			return correct;
		}
		public bool isDeadEnd() {

			return !isForwardPosReachable() && !isBackPosReachable () && !isRightPosReachable () && !isLeftPosReachable ();
		}
		public bool isGoodEnd() {

			bool res = false;
			if(LastPos().x == EndPathPosition().x && LastPos().y == EndPathPosition().y) {
				res = true;
			}


			return res;
		}

        /// <summary>
        /// Check if the move on the current context of the matrix is ​​correct
        /// </summary>
        /// <param name="move">Coordinates of the new move</param>
        /// <returns></returns>
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


		public string id() {
            string idValue = "";
            for(int i = 0; i < tracedPathPositions.Count; i++) {
                idValue = idValue + tracedPathPositions[i] + ";";
            }
            return idValue;
        }

		private void InitPathMatrix () {

            for (int r = 0; r < _pathMatrix.GetLength (0); r++) {

				for (int c = 0; c < _pathMatrix.GetLength(1); c++) {

                    _pathMatrix[r, c] = new PossibilityMatrixPathElement(new Vector2Int (r, c));
				}

			}

			for (int i = 0; i < _tracedPathPositions.Count; i++) {

                
                Vector2Int pos = _tracedPathPositions[i];
				PossibilityMatrixPathElement element = _pathMatrix[
                    pos.x,
                    pos.y
                ];
				element.SetAsTracedPos();

				_tracedPathMatrixElements.Add(element);


                // set trace direction
                if(_tracedPathPositions[i] != LastPos()) {
					Vector2Int direction = _tracedPathPositions[i + 1] - _tracedPathPositions[i];

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


	public class PossibilityMatrixPathElement {
		public PossibilityMatrixPathElement(Vector2Int pos) {
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
