using PT.DataStruct;
using PT.Global;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private SettingsController _settingsController;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameUI _gameUI;

    public static GameController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start() {

        StartGame();
    }

    private void StartGame() {

        _settingsController.InitSettings();
        PrefabManager.Instance.InitPrefabPool();
        GlobalPossibilityPath.GenerateChaptersPaths(true);
        GameSaveManager.InitSaves();

        _gameUI.OpenMainMenu();
    }

    public async void ContinueGame() {
        await StartLevel(GameSaveManager.LevelReachedInfo);
    }

    /// <summary>
    /// Load and start level
    /// </summary>
    /// <param name="levelInfo">Level info to start</param>
    /// <returns></returns>
    public async Task StartLevel(LevelInfo levelInfo) {

        
        _gameUI.CloseAllUIMenus();

        await _gameUI.SetBlackBackgroundLerp(true);

        _gameUI.OpenGameLevelStartedMenu();
        _gameUI.SetGameStateValuesUI(levelInfo);


        _levelManager.WipeLevel();
        _levelManager.LoadLevel(levelInfo);
        _levelManager.StartLevel();
        


        await _gameUI.SetBlackBackgroundLerp(false);
    }

    public void EndLevelWin() {
        GameSaveManager.SaveReachedLevel(GlobalPossibilityPath.GetNextLevel(_levelManager.LevelInfo));
        _gameUI.OpenGameLevelEndedWinMenu();
    }
    public void EndLevelLose() {
        _gameUI.OpenGameLevelEndedLoseMenu();
    }

    public async void NextLevel() {
        await StartLevel(GameSaveManager.LevelReachedInfo);
    }

    public async void RestartLevel() {
        await StartLevel(GameSaveManager.LevelReachedInfo);
    }
    
}
