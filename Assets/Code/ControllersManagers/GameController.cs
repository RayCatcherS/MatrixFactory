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


        GameUI.Instance.BuildUI();
        GameUI.Instance.OpenMainMenu();
    }
    public void LevelSelection() {
        GameUI.Instance.OpenLevelSelectionMenu();
    }

    public async void ContinueLevel() {
        await StartLevel(GameSaveManager.LevelInfoReachedInfo);
    }

    public async void MainMenu() {
        GameUI.Instance.CloseAllUIMenus();
        await GameUI.Instance.SetBlackBackgroundLerp(true);
        GameUI.Instance.OpenMainMenu();
        _levelManager.WipeLevel();
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

        if(GameSaveManager.LevelInfoReachedInfo.Chapter == GlobalPossibilityPath.Chapter.End) {

            GameUI.Instance.OpenGameEnd();

		} else {
			GameUI.Instance.OpenGameLevelWinMenu();
		}
        
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
