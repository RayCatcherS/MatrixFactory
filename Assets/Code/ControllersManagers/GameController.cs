using PT.DataStruct;
using PT.Global;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private SettingsController _settingsController;
    [SerializeField] private LevelManager _levelManager;

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

        GameUI.Instance.OpenMainMenu();
    }

    public async void ContinueGame() {
        await StartLevel(GameSaveManager.LevelInfoReachedInfo);
    }

    /// <summary>
    /// Load and start level
    /// </summary>
    /// <param name="levelInfo">Level info to start</param>
    /// <returns></returns>
    public async Task StartLevel(LevelInfo levelInfo) {

        
        GameUI.Instance.CloseAllUIMenus();

        await GameUI.Instance.SetBlackBackgroundLerp(true);

        GameUI.Instance.OpenGameLevelStartedMenu();
        GameUI.Instance.SetGameStateValuesUI(levelInfo);


        _levelManager.WipeLevel();
        _levelManager.LoadLevel(levelInfo);
        _levelManager.StartLevel();
        


        await GameUI.Instance.SetBlackBackgroundLerp(false);
    }

    public void EndLevelWin() {
        GameSaveManager.SaveReachedLevel(GlobalPossibilityPath.GetNextLevel(_levelManager.LevelInfo));
        GameUI.Instance.OpenGameLevelEndedWinMenu();
    }
    public void EndLevelLose() {
        GameUI.Instance.OpenGameLevelEndedLoseMenu();
    }

    public async void NextLevel() {
        await StartLevel(GameSaveManager.LevelInfoReachedInfo);
    }

    public async void RestartLevel() {
        await StartLevel(GameSaveManager.LevelInfoReachedInfo);
    }
    
}