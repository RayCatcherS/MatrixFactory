using PT.DataStruct;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PT.Global {
    using static GlobalPossibilityPath;

    public static class GameSaveManager {

        private static Dictionary<Chapter, int> _levelChaptersSaves;

        private static LevelInfo _levelInfoReachedInfo;
        public static LevelInfo LevelInfoReachedInfo {
            get { return _levelInfoReachedInfo; }
        }
        

        public static void InitSaves() {
            _levelChaptersSaves = new Dictionary<Chapter, int> {
                {Chapter.Chapter1, 0},
                {Chapter.Chapter2, 0},
                {Chapter.Chapter3, 0},
                {Chapter.Chapter4, 0},
                {Chapter.Chapter5, 0},
                {Chapter.Chapter6, 0}
            };

            LoadSaves();
        }

        private static async Task LoadSaves() {
            _levelInfoReachedInfo = new LevelInfo(Chapter.Chapter6, 14);
        }

        public static async void SaveReachedLevel(LevelInfo levelInfo) {
            _levelChaptersSaves[levelInfo.Chapter] = levelInfo.LevelIndex;
            _levelInfoReachedInfo = levelInfo;
            await SaveSaves();
        }

        private static async Task SaveSaves() {
            // TODO save level and chapters to file
        }
    }
}

