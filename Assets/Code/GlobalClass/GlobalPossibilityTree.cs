namespace PT.Global {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataStruct;
    using UnityEngine;

    public static class GlobalPossibilityPath {
        public enum Chapter {
            NotSelected,
            Chapter1,
            Chapter2,
            Chapter3,
            Chapter4,
            Chapter5,
            Chapter6,
            End
        }

        private static FourCTree<GeneratedLevelWithMatrix> _tree = new FourCTree<GeneratedLevelWithMatrix>();
		private static List<GeneratedLevelWithMatrix> _goodEndsDebugPathsItems = new List<GeneratedLevelWithMatrix>();


        private static List<GeneratedLevel> _goodEndsPathsC1 = new List<GeneratedLevel>();
        private static List<GeneratedLevel> _goodEndsPathsC2 = new List<GeneratedLevel>();
        private static List<GeneratedLevel> _goodEndsPathsC3 = new List<GeneratedLevel>();
        private static List<GeneratedLevel> _goodEndsPathsC4 = new List<GeneratedLevel>();
        private static List<GeneratedLevel> _goodEndsPathsC5 = new List<GeneratedLevel>();
        private static List<GeneratedLevel> _goodEndsPathsC6 = new List<GeneratedLevel>();
        private static bool _chaptersInitialized = false;


        public static FourCTree<GeneratedLevelWithMatrix> GeneratedDebugTree {
			get { return _tree; }
		}
        public static List<GeneratedLevelWithMatrix> GeneratedDebugGoodEndsPaths {
            get { return _goodEndsDebugPathsItems; }
        }
        public static List<GeneratedLevel> GetChapterLevels(Chapter chapter) {

            if(!_chaptersInitialized) {
                throw new System.InvalidOperationException("Chapter are not generated");
            }
            

            switch(chapter) {
                case Chapter.Chapter1:
                    return _goodEndsPathsC1;
                case Chapter.Chapter2:
                    return _goodEndsPathsC2;
                case Chapter.Chapter3:
                    return _goodEndsPathsC3;
                case Chapter.Chapter4:
                    return _goodEndsPathsC4;
                case Chapter.Chapter5:
                    return _goodEndsPathsC6;
                case Chapter.Chapter6:
                    return _goodEndsPathsC6;
                default:
                    return null;
            }
        }
        public static LevelInfo GetNextLevel(LevelInfo levelInfo) {

            int nextLevelIndex = levelInfo.LevelIndex + 1;

            switch(levelInfo.Chapter) {
                case Chapter.Chapter1:
                    if(levelInfo.LevelIndex == _goodEndsPathsC1.Count - 1) { 
                        return new LevelInfo(Chapter.Chapter2, 0);
                    }
                    break;
                case Chapter.Chapter2:
                    if(levelInfo.LevelIndex == _goodEndsPathsC2.Count - 1) {
                        return new LevelInfo(Chapter.Chapter3, 0);
                    }
                    break;
                case Chapter.Chapter3:
                    if(levelInfo.LevelIndex == _goodEndsPathsC3.Count - 1) {
                        return new LevelInfo(Chapter.Chapter4, 0);
                    }
                    break;
                case Chapter.Chapter4:
                    if(levelInfo.LevelIndex == _goodEndsPathsC4.Count - 1) {
                        return new LevelInfo(Chapter.Chapter5, 0);
                    }
                    break;
                case Chapter.Chapter5:
                    if(levelInfo.LevelIndex == _goodEndsPathsC6.Count - 1) {
                        return new LevelInfo(Chapter.Chapter6, 0);
                    }
                    break;
                case Chapter.Chapter6:
                    if(levelInfo.LevelIndex == _goodEndsPathsC6.Count - 1) {
                        return new LevelInfo(Chapter.End, 0);
                    }

                    break;

            }


            return new LevelInfo(levelInfo.Chapter, nextLevelIndex);
        }
        public static bool IsLastChatpterLevel(LevelInfo levelInfo) {

            switch(levelInfo.Chapter) {
                case Chapter.Chapter1:
                    return levelInfo.LevelIndex == _goodEndsPathsC1.Count - 1;
                case Chapter.Chapter2:
                    return levelInfo.LevelIndex == _goodEndsPathsC2.Count - 1;
                case Chapter.Chapter3:
                    return levelInfo.LevelIndex == _goodEndsPathsC3.Count - 1;
                case Chapter.Chapter4:
                    return levelInfo.LevelIndex == _goodEndsPathsC4.Count - 1;
                case Chapter.Chapter5:
                    return levelInfo.LevelIndex == _goodEndsPathsC6.Count - 1;
                case Chapter.Chapter6:
                    return levelInfo.LevelIndex == _goodEndsPathsC6.Count - 1;
                default:
                    return false;
            }
        }

        public static void GenerateChaptersPaths(bool debugStatistic = false, bool memorizeAllPossibilityPathTree = false) {

            double timer = 0;
            _goodEndsPathsC1 = new List<GeneratedLevel>();
            _goodEndsPathsC2 = new List<GeneratedLevel>();
            _goodEndsPathsC3 = new List<GeneratedLevel>();
            _goodEndsPathsC4 = new List<GeneratedLevel>();
            _goodEndsPathsC5 = new List<GeneratedLevel>();
            _goodEndsPathsC6 = new List<GeneratedLevel>();
            if(debugStatistic) {
                Debug.Log("Timer started");
                timer = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            }




            _goodEndsPathsC1 = GenerateChapter1(memorizeAllPossibilityPathTree)
                .OrderByDescending(item => item.score).ToList();

            _goodEndsPathsC2 = GenerateChapter2(memorizeAllPossibilityPathTree)
                .OrderByDescending(item => item.score).ToList();

            _goodEndsPathsC3 = GenerateChapter3(memorizeAllPossibilityPathTree)
                .OrderByDescending(item => item.score).ToList();

            _goodEndsPathsC4 = GenerateChapter4(memorizeAllPossibilityPathTree)
                .OrderByDescending(item => item.score).ToList();

            _goodEndsPathsC5 = GenerateChapter5(memorizeAllPossibilityPathTree)
                .OrderByDescending(item => item.score).ToList();

            _goodEndsPathsC6 = GenerateChapter6(memorizeAllPossibilityPathTree)
                .OrderByDescending(item => item.score).ToList();



            _chaptersInitialized = true;

            if(debugStatistic) {
                Debug.Log("Timer ended in: " + (System.DateTime.Now.TimeOfDay.TotalMilliseconds - timer));
            }
                
        }

        

        static private List<GeneratedLevel> GenerateChapter1(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 2; int _columns = 2;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, 1);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, 1);
            List<GeneratedLevel> goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            return goodEndsPaths1.Concat(goodEndsPaths2).ToList();
        }
        static private List<GeneratedLevel> GenerateChapter2(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 2; int _columns = 3;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, 2);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, 2);
            List<GeneratedLevel> goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            return goodEndsPaths1.Concat(goodEndsPaths2).ToList();
        }
        static private List<GeneratedLevel> GenerateChapter3(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 3; int _columns = 3;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, 2);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, 2);
            List<GeneratedLevel> goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, 2);
            List<GeneratedLevel> goodEndsPaths3 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            return goodEndsPaths1.Concat(goodEndsPaths2).Concat(goodEndsPaths3).ToList();
        }
        static private List<GeneratedLevel> GenerateChapter4(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 3; int _columns = 4;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, 3);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, 3);
            List<GeneratedLevel> goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, 3);
            List<GeneratedLevel> goodEndsPaths3 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            return goodEndsPaths1.Concat(goodEndsPaths2).Concat(goodEndsPaths3).ToList();
        }
        static private List<GeneratedLevel> GenerateChapter5(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 4; int _columns = 4;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, 3);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, 3);
            List<GeneratedLevel> goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, 3);
            List<GeneratedLevel> goodEndsPaths3 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(3, 3);
            List<GeneratedLevel> goodEndsPaths4 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            return goodEndsPaths1.Concat(goodEndsPaths2).Concat(goodEndsPaths3).Concat(goodEndsPaths4).ToList();
        }
        static private List<GeneratedLevel> GenerateChapter6(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 4; int _columns = 5;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, 4);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, 4);
            List<GeneratedLevel> goodEndsPaths2 = new List<GeneratedLevel>();
            goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, 4);
            List<GeneratedLevel> goodEndsPaths3 = new List<GeneratedLevel>();
            goodEndsPaths3 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(3, 4);
            List<GeneratedLevel> goodEndsPaths4 = new List<GeneratedLevel>();
            goodEndsPaths4 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);


            return goodEndsPaths1.Concat(goodEndsPaths2).ToList().Concat(goodEndsPaths3).Concat(goodEndsPaths4).ToList();
        }



        /// <summary>
        /// Generates all the paths that start from a point of the array and arrive at an end point
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="start">Start path point</param>
        /// <param name="end">End path point</param>
        /// <param name="memorizeAllPossibilityPathTree">Only for Debug use</param>
        /// <returns></returns>
        private static List<GeneratedLevel> GeneratePossibilitiesPathTree(int row, int column, Vector2Int start, Vector2Int end, bool memorizeAllPossibilityPathTree = false) {

            _tree = new FourCTree<GeneratedLevelWithMatrix>();
            _goodEndsDebugPathsItems = new List<GeneratedLevelWithMatrix>();


            // generate starting root path
            List<Vector2Int> startingPath = new List<Vector2Int> {
                DataDeepCopy.DeepCopy(start) /* deep copy*/
            };
            GeneratedLevelWithMatrix rootItem = new GeneratedLevelWithMatrix(row, column, start, end, startingPath);
            _tree.InsRoot(rootItem);


            if(memorizeAllPossibilityPathTree) {
                RecursivePossibilitiesPathTreeGeneration(_tree.Root());

                return GetGoodEndPathsFromPossibilityDebugTree();
            } else {

                List<GeneratedLevel> goodEndPaths = new List<GeneratedLevel>();
                RecursivePossibilitiesPathGeneration(rootItem, goodEndPaths);
                return goodEndPaths;
            }
            
            
            
        }

        private static void RecursivePossibilitiesPathGeneration(GeneratedLevelWithMatrix item, List<GeneratedLevel> _goodPaths) {

            GeneratedLevelWithMatrix pItem = item;

            if(pItem.isGoodEnd()) {
                pItem.pathMatrix[
                    pItem.LastPathPos().x,
                    pItem.LastPathPos().y
                ].SetAsGoodEnd();

                _goodPaths.Add(
                    new GeneratedLevel(
                        pItem.tracedPathMatrixElements,
                        pItem.StartPathPosition(),
                        pItem.EndPathPosition(),
                        pItem.MatrixSize()
                    )
                );    


                return;
            }

            if(pItem.isForwardPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x - 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isBackPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x + 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isRightPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y + 1
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isLeftPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y - 1
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                RecursivePossibilitiesPathGeneration(newItem, _goodPaths);
            }

            if(pItem.isDeadEnd()) {
                pItem.pathMatrix[
                    pItem.LastPathPos().x,
                    pItem.LastPathPos().y
                ].SetAsDeadEnd();
                return;
            }
        }

        private static void RecursivePossibilitiesPathTreeGeneration(FourCTreeNode<GeneratedLevelWithMatrix> node) {

            GeneratedLevelWithMatrix pItem = _tree.Read(node);

            if(pItem.isGoodEnd()) {
                pItem.pathMatrix[
                    pItem.LastPathPos().x,
                    pItem.LastPathPos().y
                ].SetAsGoodEnd();
                return;
            }

            if(pItem.isForwardPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x - 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsForward(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Forward(node));
            }

            if(pItem.isBackPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x + 1,
                        newTracedPath[newTracedPath.Count - 1].y
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsBack(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Back(node));
            }

            if(pItem.isRightPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y + 1
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsRight(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Right(node));
            }

            if(pItem.isLeftPosReachable()) {
                List<Vector2Int> newTracedPath = DataDeepCopy.DeepCopy(pItem.tracedPathPositions);
                newTracedPath.Add(
                    new Vector2Int(
                        newTracedPath[newTracedPath.Count - 1].x,
                        newTracedPath[newTracedPath.Count - 1].y - 1
                    )
                );

                GeneratedLevelWithMatrix newItem = new GeneratedLevelWithMatrix(
                    pItem.row,
                    pItem.column,
                    pItem.StartPathPosition(),
                    pItem.EndPathPosition(),
                    newTracedPath
                );

                _tree.InsLeft(
                    node,
                    newItem
                );

                RecursivePossibilitiesPathTreeGeneration(_tree.Left(node));
            }

            if(pItem.isDeadEnd()) {
                pItem.pathMatrix[
                    pItem.LastPathPos().x,
                    pItem.LastPathPos().y
                ].SetAsDeadEnd();
                return;
            }
        }

        private static List<GeneratedLevel> GetGoodEndPathsFromPossibilityDebugTree() {

            List<GeneratedLevel> goodEndsPaths = new List<GeneratedLevel>();

            Action<FourCTreeNode<GeneratedLevelWithMatrix>, FourCTree<GeneratedLevelWithMatrix>> onNodeVisit =
            (FourCTreeNode<GeneratedLevelWithMatrix> visitedNode, FourCTree<GeneratedLevelWithMatrix> tree) => {

                if(tree.Read(visitedNode).isGoodEnd()) {
                    _goodEndsDebugPathsItems.Add(tree.Read(visitedNode));

                    goodEndsPaths.Add(
                        new GeneratedLevel(
                            tree.Read(visitedNode).tracedPathMatrixElements,
                            tree.Read(visitedNode).StartPathPosition(),
                            tree.Read(visitedNode).EndPathPosition(),
                            tree.Read(visitedNode).MatrixSize()
                        )
                    );
                }
            };

            _tree.VisitTree(_tree.Root(), onNodeVisit);


            /*foreach(var element in _goodEndsPathsDebug) {
				goodEndsPaths.Add(
                    new GoodEndPath(
                        element.tracedPathMatrixElements,
						element.StartPathPosition(),
						element.EndPathPosition(),
						element.MatrixSize()
                    )
                );
            }*/

            return goodEndsPaths;
        }
    }
}
