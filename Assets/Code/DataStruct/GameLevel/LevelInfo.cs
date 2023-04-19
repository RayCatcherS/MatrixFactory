using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PT.DataStruct {
    using static Global.GlobalPossibilityPath;
    public class LevelInfo {
        public LevelInfo(Chapter chapter, int levelIndex) {
            _chapter = chapter;
            _levelIndex = levelIndex;
        }
        private Chapter _chapter;
        public Chapter Chapter {
            get { return _chapter; }
        }
        private int _levelIndex;
        public int LevelIndex {
            get { return _levelIndex; }
        }

        public string GetChapterName() {

            switch(_chapter) {
                case Chapter.Chapter1:
                    return "1";
                case Chapter.Chapter2:
                    return "2";
                case Chapter.Chapter3:
                    return "3";
                case Chapter.Chapter4:
                    return "4";
                case Chapter.Chapter5:
                    return "5";
                case Chapter.Chapter6:
                    return "6";
                default:
                    return "-1";
            }
        }
        public string GetLevelName() {
            return (_levelIndex + 1).ToString();
        }
    }
}

