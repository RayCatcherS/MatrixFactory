using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PT.DataStruct {
    public class GeneratedLevel : Path {
        public GeneratedLevel(List<PossibilityMatrixPathElement> tracedPathPositions, Vector2Int startPathPosition, Vector2Int endPathPosition, Vector2Int matrixSize) {


            _startPathPosition = new Vector2Int(startPathPosition.x, startPathPosition.y);
            _endPathPosition = new Vector2Int(endPathPosition.x, endPathPosition.y);
            _matrixSize = matrixSize;


            InitGoodEndPath(tracedPathPositions);
        }
        private List<GoodEndPathElement> _path = new List<GoodEndPathElement>();
        private Vector2Int _startPathPosition;
        private Vector2Int _endPathPosition;
        private Vector2Int _matrixSize;
        private double _score = 0;


        public Vector2Int MatrixSize() {
            return _matrixSize;
        }
        public List<GoodEndPathElement> PathElements {
            get { return _path; }
        }
        public int PathLenght {
            get { return _path.Count; }
        }
        

        public Vector2Int LastPos() {
            return _path[_path.Count - 1].pos;
        }
        private void InitGoodEndPath(List<PossibilityMatrixPathElement> posPath) {

            Direction direction = posPath[0].tracedMoveDirection; // first direction move
            int directionChanges = 0;

            foreach(PossibilityMatrixPathElement pos in posPath) {

                if(direction != pos.tracedMoveDirection) {
                    direction = pos.tracedMoveDirection;
                    directionChanges++;


                }
                _path.Add(
                    new GoodEndPathElement(pos.tracedMoveDirection, pos.pos)
                );
            }

            // score
            double starEndDistanceMalus = Math.Abs(_startPathPosition.x - _endPathPosition.x) + Math.Abs(_startPathPosition.y - _endPathPosition.y);
            double pathLengthMalus = _path.Count;
            double malus = Math.Pow(directionChanges, 1.9f) + Math.Pow(starEndDistanceMalus, 1.15f) + Math.Pow(pathLengthMalus, 1.4f);
            _score =  ((_matrixSize.x * _matrixSize.y) / malus) ;

        }
        public Vector2Int EndPathPosition() {
            return _endPathPosition;
        }
        public Vector2Int StartPathPosition() {
            return _startPathPosition;
        }
        public double score {
            get { return _score; }
        }

        public int packageToSpawn {
            get { return _path.Count() * 2; }
        }

        public string id() {
            string idValue = "";
            for(int i = 0; i < _path.Count; i++) {
                idValue = idValue + _path[i].pos + ";";
            }
            return idValue;
        }
    }

    public class GoodEndPathElement {
        public GoodEndPathElement(Direction move, Vector2Int pos) {
            _move = move;
            _pos = pos;
        }
        private Direction _move;
        private Vector2Int _pos;

        public Vector2Int pos {
            get { return _pos; }
        }
    }
}
