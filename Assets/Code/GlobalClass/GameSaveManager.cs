using PT.DataStruct;
using System.Threading.Tasks;
using UnityEngine;

namespace PT.Global {
    using static GlobalPossibilityPath;

    public static class GameSaveManager {

        private static LevelInfo _currentLevelReached;
        private static LevelInfo _globalLevelReached;

        readonly private static string _currentChapterReachedKeySave = "currentChapterReached";
        readonly private static string _currentLevelReachedKeySave = "currentLevelReached";
        readonly private static string _globalChapterReachedKeySave = "globalChapterReached";
        readonly private static string _globalLevelReachedKeySave = "globalLevelReached";

        private static int _selectedGraphicSetting = 2;

        public static LevelInfo CurrentReachedLevel {
            get { return _currentLevelReached; }
        }
        public static LevelInfo GlobalLevelReached {
            get { return _globalLevelReached; }
        }
        

        public static void InitSavesAsync() {
            
            LoadSaves();
        }

        private static void LoadSaves() {

            // if no saves file use default start
            if(PlayerPrefs.HasKey(_currentChapterReachedKeySave) 
                && PlayerPrefs.HasKey(_currentLevelReachedKeySave)
                && PlayerPrefs.HasKey(_globalChapterReachedKeySave)
                && PlayerPrefs.HasKey(_globalLevelReachedKeySave)
            ) {

                
                _currentLevelReached = new LevelInfo(
                GetChapterByIndex(PlayerPrefs.GetInt(_currentChapterReachedKeySave)),
                PlayerPrefs.GetInt(_currentLevelReachedKeySave)
                );

                _globalLevelReached = new LevelInfo(
                    GetChapterByIndex(PlayerPrefs.GetInt(_globalChapterReachedKeySave)),
                    PlayerPrefs.GetInt(_globalLevelReachedKeySave)
                );
            } else {
                DefaultStart();
            }

        }

        private static void DefaultStart() {

            _globalLevelReached = new LevelInfo(Chapter.Chapter1, 0);
            _currentLevelReached = new LevelInfo(Chapter.Chapter1, 0);
        }

        public static void SetCurrentReachedLevel(LevelInfo levelInfo) {
            _currentLevelReached = levelInfo;

            SaveCurrentReachedLevelState(_currentLevelReached);


            // check if global reached level is lower than current reached level and save it
            if(GetChapterIndex(_currentLevelReached.Chapter) == GetChapterIndex(_globalLevelReached.Chapter)) {
            
                if(_currentLevelReached.LevelIndex > _globalLevelReached.LevelIndex) {

                    SaveGlobalReachedLevelState(_currentLevelReached);
                }
            } else if(GetChapterIndex(_currentLevelReached.Chapter) > GetChapterIndex(_globalLevelReached.Chapter)) {
                SaveGlobalReachedLevelState(_currentLevelReached);
            }
        }


        private static void SaveGlobalReachedLevelState(LevelInfo levelInfo) {
            
            _globalLevelReached = levelInfo;

            PlayerPrefs.SetInt("globalChapterReached", (int)GetChapterIndex(_globalLevelReached.Chapter));
            PlayerPrefs.SetInt("globalLevelReached", _globalLevelReached.LevelIndex);
            PlayerPrefs.Save();
        }
        private static void SaveCurrentReachedLevelState(LevelInfo levelInfo) {

            _currentLevelReached = levelInfo;
            PlayerPrefs.SetInt("currentChapterReached", (int)GetChapterIndex(_currentLevelReached.Chapter));
            PlayerPrefs.SetInt("currentLevelReached", _currentLevelReached.LevelIndex);
            PlayerPrefs.Save();
        }
    
        public static void SetGraphicSettings(int graphicSettings) {
            _selectedGraphicSetting = graphicSettings;
        }
        public static void SaveGraphicSettings() {
            QualitySettings.SetQualityLevel(_selectedGraphicSetting, true);
            PlayerPrefs.Save();

            Debug.Log("Graphic settings saved: " + QualitySettings.GetQualityLevel());
        }
    }
}

