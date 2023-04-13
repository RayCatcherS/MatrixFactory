
using PT.DataStruct;
using PT.Global;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private SettingsController _settingsController;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameUI _gameUI;

    void Start() {

        StartGame();
    }

    private void StartGame() {

        _settingsController.InitSettings();
        GlobalPossibilityPath.GenerateChaptersPaths(true);
        GameSaveManager.InitSaves();

        _gameUI.OpenMainMenu();
    }

    public async void ContinueGame() {
        await StartLevel(GameSaveManager.LevelReachedInfo);
    }

    public async Task StartLevel(LevelInfo levelInfo) {

        
        _gameUI.CloseAllUIMenus();

        await _gameUI.SetBlackBackgroundLerp(true);

        _gameUI.OpenGameLevelStartedMenu();
        _gameUI.SetGameStateValuesUI(levelInfo);


        _levelManager.DestroyLevel();
        _levelManager.InitLevel(levelInfo.Chapter, levelInfo.LevelIndex);
        _levelManager.StartLevel();
        


        await _gameUI.SetBlackBackgroundLerp(false);
    }

    public void EndLevelWin() {
        GameSaveManager.SaveReachedLevel(GlobalPossibilityPath.GetNextLevel(_levelManager.LevelInfo));
        _gameUI.OpenGameLevelEndedWinMenu();
    }

    public async void NextLevel() {
        await StartLevel(GameSaveManager.LevelReachedInfo);
    }

    public async void RestartLevel() {
        await StartLevel(GameSaveManager.LevelReachedInfo);
    }

    public void EndLevelLose() {
        _gameUI.OpenGameLevelEndedLoseMenu();
    }
}
