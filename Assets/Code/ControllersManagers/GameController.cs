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

        StartMainAsync();
    }

    private void StartMainAsync() {

        _settingsController.InitSettings();
        PrefabManager.Instance.InitPrefabPool();
        GlobalPossibilityPath.GenerateChaptersPaths(true);
        GameSaveManager.InitSavesAsync();


        GameUI.Instance.OpenMainMenu();
    }
    public void LevelSelection() {
        GameUI.Instance.OpenLevelSelectionMenu();
    }

    public async void ContinueLevel() {
        await StartLevel(GameSaveManager.CurrentReachedLevel);
    }

    public async void MainMenu() {
        GameUI.Instance.CloseAndResetAllUIMenus();
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

        
        GameUI.Instance.CloseAndResetAllUIMenus();

        await GameUI.Instance.SetBlackBackgroundLerp(true);

        GameUI.Instance.OpenGameLevelStartedMenu(levelInfo);
        GameUI.Instance.SetGameStateValuesUI(levelInfo);


        _levelManager.WipeLevel();
        _levelManager.LoadLevel(levelInfo);


        gameObject.GetComponent<ControlsController>().enabled = false;
        await gameObject.GetComponent<Tutorial>().CheckTutorial(levelInfo);
        _levelManager.StartLevel();
        gameObject.GetComponent<ControlsController>().enabled = true;




        await GameUI.Instance.SetBlackBackgroundLerp(false);
    }

    public void EndLevelWin() {
        GameSaveManager.SetCurrentReachedLevel(GlobalPossibilityPath.GetNextLevel(_levelManager.LevelInfo));

        if(GameSaveManager.CurrentReachedLevel.Chapter == GlobalPossibilityPath.Chapter.End) {

            GameUI.Instance.OpenGameEnd();

		} else {
			GameUI.Instance.OpenGameLevelWinMenu();
            _levelManager.DrawUI();

        }
        
    }
    public void EndLevelLose() {
        GameUI.Instance.OpenGameLevelEndedLoseMenu();
    }

    public async void NextLevel() {
        await StartLevel(GameSaveManager.CurrentReachedLevel);
    }

    public async void RestartLevel() {
        await StartLevel(GameSaveManager.CurrentReachedLevel);
    }
    
    
}
