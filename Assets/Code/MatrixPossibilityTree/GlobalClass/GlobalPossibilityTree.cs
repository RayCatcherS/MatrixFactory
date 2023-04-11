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

        private static FourCTree<PossibilityPathItem> _tree = new FourCTree<PossibilityPathItem>();
		private static List<PossibilityPathItem> _goodEndsDebugPathsItems = new List<PossibilityPathItem>();


        private static List<GeneratedLevel> _goodEndsPathsC1 = new List<GeneratedLevel>();
        private static List<GeneratedLevel> _goodEndsPathsC6 = new List<GeneratedLevel>();
        private static bool _chaptersInitialized = false;


        public static FourCTree<PossibilityPathItem> GeneratedDebugTree {
			get { return _tree; }
		}
        public static List<PossibilityPathItem> GeneratedDebugGoodEndsPaths {
            get { return _goodEndsDebugPathsItems; }
        }
        public static List<GeneratedLevel> GetChapterLevels(Chapter chapter) {

            if(!_chaptersInitialized) {
                throw new System.InvalidOperationException("Chapter are not generated");
            }
            

            switch(chapter) {
                case Chapter.Chapter1:
                    return _goodEndsPathsC1;
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
                case Chapter.Chapter6:
                    return levelInfo.LevelIndex == _goodEndsPathsC6.Count - 1;
                default:
                    return false;
            }
        }




        public static void GenerateChaptersPaths(bool debugStatistic = false, bool memorizeAllPossibilityPathTree = false) {

            double timer = 0;
            _goodEndsPathsC1 = new List<GeneratedLevel>();
            _goodEndsPathsC6 = new List<GeneratedLevel>();
            if(debugStatistic) {
                Debug.Log("Timer started");
                timer = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            }




            _goodEndsPathsC1 = GenerateChapter1(memorizeAllPossibilityPathTree)
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
            endPathPosition = new Vector2Int(0, 1);
            List<GeneratedLevel> goodEndsPaths2 = new List<GeneratedLevel>();
            goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);

            return goodEndsPaths1.Concat(goodEndsPaths2).ToList();
        }
        static private List<GeneratedLevel> GenerateChapter6(bool memorizeAllPossibilityPathTree = false) {

            int _rows = 4; int _columns = 5;

            Vector2Int startPathPosition = new Vector2Int(0, 0);
            Vector2Int endPathPosition = new Vector2Int(0, _columns - 1);
            List<GeneratedLevel> goodEndsPaths1 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition, memorizeAllPossibilityPathTree);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(1, _columns - 1);
            List<GeneratedLevel> goodEndsPaths2 = new List<GeneratedLevel>();
            goodEndsPaths2 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);


            startPathPosition = new Vector2Int(0, 0);
            endPathPosition = new Vector2Int(2, _columns - 1);
            List<GeneratedLevel> goodEndsPaths3 = new List<GeneratedLevel>();
            goodEndsPaths3 = GeneratePossibilitiesPathTree(_rows, _columns, startPathPosition, endPathPosition);


            return goodEndsPaths1.Concat(goodEndsPaths2).ToList().Concat(goodEndsPaths3).ToList();
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

            _tree = new FourCTree<PossibilityPathItem>();
            _goodEndsDebugPathsItems = new List<PossibilityPathItem>();


            // generate starting root path
            List<Vector2Int> startingPath = new List<Vector2Int> {
                DataDeepCopy.DeepCopy(start) /* deep copy*/
            };
            PossibilityPathItem rootItem = new PossibilityPathItem(row, column, start, end, startingPath);
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

        private static void RecursivePossibilitiesPathGeneration(PossibilityPathItem item, List<GeneratedLevel> _goodPaths) {

            PossibilityPathItem pItem = item;

            if(pItem.isGoodEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsDeadEnd();
                return;
            }
        }

        private static void RecursivePossibilitiesPathTreeGeneration(FourCTreeNode<PossibilityPathItem> node) {

            PossibilityPathItem pItem = _tree.Read(node);

            if(pItem.isGoodEnd()) {
                pItem.pathMatrix[
                    pItem.LastPos().x,
                    pItem.LastPos().y
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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

                PossibilityPathItem newItem = new PossibilityPathItem(
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
                    pItem.LastPos().x,
                    pItem.LastPos().y
                ].SetAsDeadEnd();
                return;
            }
        }

        private static List<GeneratedLevel> GetGoodEndPathsFromPossibilityDebugTree() {

            List<GeneratedLevel> goodEndsPaths = new List<GeneratedLevel>();

            Action<FourCTreeNode<PossibilityPathItem>, FourCTree<PossibilityPathItem>> onNodeVisit =
            (FourCTreeNode<PossibilityPathItem> visitedNode, FourCTree<PossibilityPathItem> tree) => {

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
