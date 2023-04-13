using PT.DataStruct;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PT.Global {
    using static GlobalPossibilityPath;

    public static class GameSaveManager {

        private static Dictionary<Chapter, int> _levelChaptersSaves;

        private static LevelInfo _levelReachedInfo;
        public static LevelInfo LevelReachedInfo {
            get { return _levelReachedInfo; }
        }

        public static int GetLevelChapterSave(Chapter chapter) {
            return _levelChaptersSaves[chapter];
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
            _levelReachedInfo = new LevelInfo(Chapter.Chapter1, 0);
        }

        public static async void SaveReachedLevel(LevelInfo levelInfo) {
            _levelChaptersSaves[levelInfo.Chapter] = levelInfo.LevelIndex;
            _levelReachedInfo = levelInfo;
            await SaveSaves();
        }

        private static async Task SaveSaves() {
            // TODO save level and chapters to file
        }
    }
}

