using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PT.DataStruct {
    public class GeneratedLevel : Path {
        public GeneratedLevel(List<GeneratedLevelWithMatrixElement> tracedPathPositions,
            Vector2Int startPathPosition,
            Vector2Int endPathPosition,
            Vector2Int matrixSize
        ) {

            _startPathPosition = new Vector2Int(startPathPosition.x, startPathPosition.y);
            _endPathPosition = new Vector2Int(endPathPosition.x, endPathPosition.y);
            _matrixSize = matrixSize;


            InitPath(tracedPathPositions);
        }
        private List<LevelPathElement> _path = new List<LevelPathElement>();
        private Vector2Int _startPathPosition;
        private Vector2Int _endPathPosition;
        private Vector2Int _matrixSize;
        private double _score = 0;


        public Vector2Int MatrixSize() {
            return _matrixSize;
        }
        public List<LevelPathElement> PathElements {
            get { return _path; }
        }
        public int PathLenght {
            get { return _path.Count; }
        }
        

        public Vector2Int LastPathPos() {
            return _path[_path.Count - 1].pos;
        }
        private void InitPath(List<GeneratedLevelWithMatrixElement> posPath) {

            Direction direction = posPath[0].moveDirection; // first direction move
            int directionChanges = 0;

            int elevatorGenerationOffset = 4;

            /* INIT PATH ELEMENTS*/
            for(int i = 0; i < posPath.Count; i++) {

                /* count direction changes in the path, used to set level score*/
                if(direction != posPath[i].moveDirection) {
                    direction = posPath[i].moveDirection;
                    directionChanges++;
                }



                /* SET ELEVATORS ON THE PATH*/
                if(i != posPath.Count - 1 && i != 0) { // not the last path item
                    if(i % elevatorGenerationOffset == 0 ) {
                        _path.Add(
                        new LevelPathElement(
                            posPath[i].moveDirection,
                            posPath[i].pos,
                            ConveyorBelt.ConveyorBeltPlatformType.ElevatorCannon
                            )
                        );
                    } else {
                        _path.Add(
                        new LevelPathElement(
                            posPath[i].moveDirection,
                            posPath[i].pos,
                            ConveyorBelt.ConveyorBeltPlatformType.Roller
                            )
                        );
                    }


                } else { // last path item
                    _path.Add(
                     new LevelPathElement(
                         posPath[i].moveDirection,
                         posPath[i].pos,
                         ConveyorBelt.ConveyorBeltPlatformType.Roller
                     )
                 );
                }
            }



            /* SET PATH LEVEL SCORE*/
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
            get { return _path.Count() * 3; }
        }

        public string id() {
            string idValue = "";
            for(int i = 0; i < _path.Count; i++) {
                idValue = idValue + _path[i].pos + ";";
            }
            return idValue;
        }
    }

    public class LevelPathElement {
        public LevelPathElement(Direction move, Vector2Int pos, ConveyorBelt.ConveyorBeltPlatformType conveyorBeltPlatformType) {
            _move = move;
            _pos = pos;
            _conveyorBeltPlatformType = conveyorBeltPlatformType;
        }
        private Direction _move;
        private Vector2Int _pos;
        ConveyorBelt.ConveyorBeltPlatformType _conveyorBeltPlatformType;

        public Vector2Int pos {
            get { return _pos; }
        }

        public ConveyorBelt.ConveyorBeltPlatformType conveyorBeltPlatformType {
            get {
                return _conveyorBeltPlatformType;
            } 
            set {
                _conveyorBeltPlatformType = value;
            }
        }
    }
}
