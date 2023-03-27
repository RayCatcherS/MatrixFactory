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

				if (_pathMatrix[move.x, move.y].markedPos) {
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
				idValue = idValue + pathMatrix[matrixReachedPos.x, matrixReachedPos.y].deadEnd.ToString();
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
				matrix [
					pos.x,
					pos.y
				].setAsMarkedPos();
			}
		}
	}


	public class PathMatrixElement {
		public PathMatrixElement(Vector2Int pos) {
			_pos = pos;
		}
		private Vector2Int _pos;
		private bool _markedPos = false;
		private bool _deadEnd = false;
		private bool _goodEnd = false;
		public bool deadEnd {
			get { return _deadEnd; }
		}
		public bool goodEnd {
			get { return _goodEnd; }
		}

		public bool markedPos {
			get { return _markedPos; }
		}

		public Vector2Int pos {
			get { return _pos; }
		}

		public void setAsDeadEnd() {
			_deadEnd = true;
		}
		public void setAsGoodEnd() {
			_goodEnd = true;
		}
		public void setAsMarkedPos() {
			_markedPos = true;
		}
	}
}
