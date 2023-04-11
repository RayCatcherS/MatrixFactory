
using PT.DataStruct;
using PT.Global;
using System.Threading.Tasks;
using UnityEngine;
using static PT.Global.GlobalPossibilityPath;

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
        await StartLevel(GameSaveManager.CurrentLevelInfo);
    }

    public async Task StartLevel(LevelInfo levelInfo) {

        _gameUI.OpenGameLevelStarted();
        await _gameUI.SetBlackBackgroundLerp(true);


        _levelManager.DestroyLevel();
        _levelManager.InitLevel(levelInfo.Chapter, levelInfo.LevelIndex);
        _levelManager.StartLevel();


        await _gameUI.SetBlackBackgroundLerp(false);
    }

    public void EndLevelWin() {
        GameSaveManager.SaveChapter(_levelManager.LevelInfo);
        _gameUI.OpenGameLevelEndedWin();
    }

    public async void NextLevel() {
        await StartLevel(GlobalPossibilityPath.GetNextLevel(GameSaveManager.CurrentLevelInfo));
    }

    public void EndLevelLose() {
        _gameUI.OpenGameLevelEndedLose();
    }
}
