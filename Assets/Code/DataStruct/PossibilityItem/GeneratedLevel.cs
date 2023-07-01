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
            InitLevelPackageSequence();
            InitLevelPackageTutorial1Sequence();
        }
        private List<LevelPathElement> _path = new List<LevelPathElement>();
        private Vector2Int _startPathPosition;
        private Vector2Int _endPathPosition;
        private Vector2Int _matrixSize;
        private double _score = 0;
        private List<Package.PackageType> _levelPackagesSequence = new List<Package.PackageType>();
        private List<Package.PackageType> _levelTutorial1PackagesSequence = new List<Package.PackageType>();

        // Number of default roller platform before the elevator platform
        private int _elevatorInPathGenerationOffset = 3;


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

            

            /* INIT PATH ELEMENTS*/
            for(int i = 0; i < posPath.Count; i++) {

                /* count direction changes in the path, used to set level score*/
                if(direction != posPath[i].moveDirection) {
                    direction = posPath[i].moveDirection;
                    directionChanges++;
                }



                
                if(i != posPath.Count - 1 && i != 0) { // not the last path item

                    /* SET ELEVATORS ON THE PATH*/
                    if(i % _elevatorInPathGenerationOffset == 0 ) {
                        _path.Add(
                        new LevelPathElement(
                            posPath[i].moveDirection,
                            posPath[i].pos,
                            ConveyorBelt.PlatformType.ElevatorCannon
                            )
                        );
                    } else {
                        _path.Add(
                        new LevelPathElement(
                            posPath[i].moveDirection,
                            posPath[i].pos,
                            ConveyorBelt.PlatformType.Roller
                            )
                        );
                    }


                } else {

                    /* SET INCINERATOR ON THE PATH*/
                    if(i == posPath.Count - 1) {
                        _path.Add(
                            new LevelPathElement(
                            posPath[i].moveDirection,
                            posPath[i].pos,
                            ConveyorBelt.PlatformType.Incinerator
                            )
                        );
                    } else {
                        /* SET DEFAULT PLATFORM ON THE PATH*/
                        _path.Add(
                            new LevelPathElement(
                            posPath[i].moveDirection,
                            posPath[i].pos,
                            ConveyorBelt.PlatformType.Roller
                            )
                        );
                    }
                }
            }



            /* SET PATH LEVEL SCORE*/
            double starEndDistanceMalus = Math.Abs(_startPathPosition.x - _endPathPosition.x) + Math.Abs(_startPathPosition.y - _endPathPosition.y);
            double pathLengthMalus = _path.Count;
            double malus = Math.Pow(directionChanges, 1.9f) + Math.Pow(starEndDistanceMalus, 1.15f) + Math.Pow(pathLengthMalus, 1.4f);
            _score =  ((_matrixSize.x * _matrixSize.y) / malus) ;

        }
        
        private void InitLevelPackageSequence() {

            /* SET PACKAGES */
            for(int i = 0; i < BombPackageToSpawn; i++) {
                _levelPackagesSequence.Add(Package.PackageType.bomb);
            }

            for(int i = 0; i < TotalPackageToSpawn - BombPackageToSpawn; i++) {
                _levelPackagesSequence.Add(Package.PackageType.normal);
            }



            /* SHUFFLE PACKAGES SEQUENCE */
            System.Random rnd = new System.Random();
            _levelPackagesSequence = _levelPackagesSequence.OrderBy(x => rnd.Next()).ToList();
        }
        private void InitLevelPackageTutorial1Sequence() {

            for(int i = 0; i < TotalPackageToSpawn; i++) {
                _levelTutorial1PackagesSequence.Add(Package.PackageType.normal);
            }
        }

        public Vector2Int EndPathPosition() {
            return _endPathPosition;
        }
        public Vector2Int StartPathPosition() {
            return _startPathPosition;
        }

        /// <summary>
        /// The height of the first platform of the path
        /// It is used to generate the level
        /// </summary>
        public int StartingLevelHeightPlatform {
            get {
                return _elevatorInPathGenerationOffset * DefaultPlatformHeightDecrementer;
            }
        }

        /// <summary>
        /// Elevator platform height decrementer
        /// 
        /// It represents the value of how much the height of the 
        /// platform following an elevator-type platform must be decreased
        /// </summary>
        public int ElevatorPlatformHeightDecrementer {
            get {
                return 6; 
            }
        }
        public int DefaultPlatformHeightDecrementer {
            get {
                return 1;
            }
        }

        public int NumberOfElevatorInLevel {
            get {
                return (PathElements.Count - 1) / _elevatorInPathGenerationOffset;
            }
        }

        public double score {
            get { return _score; }
        }

        public int TotalPackageToSpawn {
            get { return _path.Count() * 3; }
        }
        public int TotalSafePackage {
            get { return TotalPackageToSpawn - BombPackageToSpawn; }
        }
        private int BombPackageToSpawn {
            get { return TotalPackageToSpawn / 4; }
        }
        public int NumberOfPackageToWin {
            get { return (int)(TotalSafePackage / 1.5f); }
        }
        
        public List<Package.PackageType> PackagesSequence {
            get {
                return _levelPackagesSequence;
            }
        }
        public List<Package.PackageType> Tutorial1PackagesSequence {
            get {
                return _levelTutorial1PackagesSequence;
            }
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
        public LevelPathElement(Direction move, Vector2Int pos, ConveyorBelt.PlatformType conveyorBeltPlatformType) {
            _move = move;
            _pos = pos;
            _conveyorBeltPlatformType = conveyorBeltPlatformType;
        }
        private Direction _move;
        private Vector2Int _pos;
        ConveyorBelt.PlatformType _conveyorBeltPlatformType;

        public Vector2Int pos {
            get { return _pos; }
        }

        public ConveyorBelt.PlatformType conveyorBeltPlatformType {
            get {
                return _conveyorBeltPlatformType;
            } 
            set {
                _conveyorBeltPlatformType = value;
            }
        }
    }
}
