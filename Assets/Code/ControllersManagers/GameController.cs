using PT.DataStruct;
using PT.Global;
using System.Threading.Tasks;
using UnityEngine;

public enum LevelDebugPackageSetting {
    None,
    InfinitePackages,
    OnePackage,
}

public class GameController : MonoBehaviour {
    
    [SerializeField] private SettingsController _settingsController;
    [SerializeField] private LevelManager _levelManager;

    [Header("Main Game Sounds References")]
    [SerializeField] private AudioClip _levelLose;
    [SerializeField] private AudioClip _levelWin;

    [Header("Debug Settings")]
    [SerializeField] bool _levelDisableRandomPath = false;
    [SerializeField] private LevelDebugPackageSetting _levelDebugPackageSetting;
    [SerializeField] private bool _disableLevelDrawPackageTrails = false;

    public static GameController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    async Task Start() {

        InitGame();

        //await StartBenchmarkLevel();
        MainMenu();
    }

    private void InitGame() {

        _settingsController.InitSettings();
        PrefabManager.Instance.InitPrefabPool();
        GlobalPossibilityPath.GenerateChaptersPaths(true);
        GameSaveManager.InitSavesAsync();
        
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
    public async Task StartLevel(LevelInfo levelInfo, bool startInbackground = false) {

        if(!startInbackground) {
            GameUI.Instance.CloseAndResetAllUIMenus();

            await GameUI.Instance.SetBlackBackgroundLerp(true);

            GameUI.Instance.OpenGameLevelStartedMenu(levelInfo);
            GameUI.Instance.SetGameStateValuesUI(levelInfo);
        }
        

        


        _levelManager.WipeLevel();
        _levelManager.LoadLevel(levelInfo, _levelDebugPackageSetting, _levelDisableRandomPath, _disableLevelDrawPackageTrails);


        gameObject.GetComponent<ControlsController>().enabled = false;

        if(!startInbackground) {
            await gameObject.GetComponent<Tutorial>().CheckTutorial(levelInfo);
        }
            
        _levelManager.StartLevel();

        if(!startInbackground) {
            gameObject.GetComponent<ControlsController>().enabled = true;

            await GameUI.Instance.SetBlackBackgroundLerp(false);
        }
        

        
    }

    public void EndLevelWin() {
        GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _levelWin;
        audioSource.GetComponent<AudioSource>().Play();


        GameSaveManager.SetCurrentReachedLevel(GlobalPossibilityPath.GetNextLevel(_levelManager.LevelInfo));

        if(GameSaveManager.CurrentReachedLevel.Chapter == GlobalPossibilityPath.Chapter.End) {

            GameUI.Instance.OpenGameEnd();

		} else {
			GameUI.Instance.OpenGameLevelWinMenu();
            _levelManager.DrawUI();

        }
        
    }
    public void EndLevelLose() {
        GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _levelLose;
        audioSource.GetComponent<AudioSource>().Play();


        GameUI.Instance.OpenGameLevelEndedLoseMenu();
    }

    public async void NextLevel() {
        await StartLevel(GameSaveManager.CurrentReachedLevel);
    }

    public async void RestartLevel() {
        await StartLevel(GameSaveManager.CurrentReachedLevel);
    }
    
    public async Task StartBenchmarkLevel() {
        _levelDebugPackageSetting = LevelDebugPackageSetting.InfinitePackages;
        _levelDisableRandomPath = true;
        LevelInfo lastLevel = new LevelInfo(GlobalPossibilityPath.Chapter.Chapter6, 15);
        await StartLevel(lastLevel, true);
    }

    public void OpenGameSettings() {
        GameUI.Instance.OpenGameSettings();
    }

    
}
