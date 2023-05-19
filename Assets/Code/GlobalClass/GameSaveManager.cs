using PT.DataStruct;
using System.Threading.Tasks;
using UnityEngine;

namespace PT.Global {
    using static GlobalPossibilityPath;

    public static class GameSaveManager {

        private static LevelInfo _currentLevelReached;
        private static LevelInfo _globalLevelReached;


        public static LevelInfo CurrentReachedLevel {
            get { return _currentLevelReached; }
        }
        public static LevelInfo GlobalLevelReached {
            get { return _globalLevelReached; }
        }
        

        public static void InitSaves() {
            
            LoadSaves();
        }

        private static async Task LoadSaves() {
            _globalLevelReached = _currentLevelReached = new LevelInfo(Chapter.Chapter1, 0);
        }

        public static async void SetCurrentReachedLevel(LevelInfo levelInfo) {
            _currentLevelReached = levelInfo;

            if(GetChapterIndex(_currentLevelReached.Chapter) == GetChapterIndex(_globalLevelReached.Chapter)) {
            
                if(_currentLevelReached.LevelIndex > _globalLevelReached.LevelIndex) {

                    await SaveGameState(_currentLevelReached);
                }
            } else if(GetChapterIndex(_currentLevelReached.Chapter) > GetChapterIndex(_globalLevelReached.Chapter)) {
                await SaveGameState(_currentLevelReached);
            }
        }


        private static async Task SaveGameState(LevelInfo levelInfo) {
            
            _globalLevelReached = levelInfo;

            // TODO save _globalReachedLevel to file
            Debug.Log("Save Game");
        }
    }
}

