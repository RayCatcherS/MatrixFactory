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
    }
}

