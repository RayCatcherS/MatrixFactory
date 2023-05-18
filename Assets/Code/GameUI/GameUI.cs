using PT.DataStruct;
using PT.Global;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [Header("background & Settings")]
    [SerializeField] private Image _background;
    private bool _isBackgroundEnable = true;
    private Color _targetColor;
    [SerializeField] private AnimationCurve _backgroundtransitionLerpCurve;
    private float _backgroundAnimationTransitionTime;
    private float _backgroundAnimationSpeed = 0.2f;


    [Header("UI Menus")]
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _levelEndedWinMenuUI;
    [SerializeField] private GameObject _levelEndedLoseMenuUI;
    [SerializeField] private GameObject _levelSelectionUI;

    [SerializeField] private GameObject _levelStartedMenuUI;
    [SerializeField] private GameObject _levelStartedGameStateUI;
    [SerializeField] private Text _levelStartedLevelNameText;
    [SerializeField] private Text _levelStartedChapterNameText;
    [SerializeField] private Text _packageToSpawnText;

    [Header("List Reference")]
    [SerializeField] private GameObject _levelListUI;

    [Header("Prefab References")]
    [SerializeField] private GameObject _buttonLevelUI;
    [SerializeField] private GameObject _textChapterTitleLevelUI;

    readonly private string _levelStartedLevelNameTextFormat = "Level: ";
    readonly private string _levelStartedChapterNameTextFormat = "Chapter: ";


    private GameUIState _state;
    public GameUIState State => _state;

    public static GameUI Instance { get; private set; }
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;

        
    }
    public void BuildUI() {
        BuildLevelListUI();
    }

    public async Task SetBlackBackgroundLerp(bool isBlackBackground) {


        if(isBlackBackground) {

            _targetColor = Color.black;
        } else {

            _targetColor = Color.clear;
        }


        if(isBlackBackground != _isBackgroundEnable) {
            _backgroundAnimationTransitionTime = 0;
            StopAllCoroutines();
            await LerpLoop();
        }

        _isBackgroundEnable = isBlackBackground;

        return;
    }

    private async Task LerpLoop() {

        while(_background.color != _targetColor) {

            _backgroundAnimationTransitionTime += _backgroundAnimationSpeed * Time.deltaTime;
            _background.color = Color.Lerp(_background.color, _targetColor, _backgroundAnimationTransitionTime);
            await Task.Yield();
        }
        
    }
    


    public void OpenMainMenu() {
        _state = GameUIState.MainMenu;
        _mainMenuUI.SetActive(true);

        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);

        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(false);

        _levelSelectionUI.SetActive(false);
    }

    public void OpenGameLevelStartedMenu() {
        _state = GameUIState.LevelStartedMenu;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);

        _levelStartedMenuUI.SetActive(true);
        _levelStartedGameStateUI.SetActive(true);

        _levelSelectionUI.SetActive(false);
    }

    public void OpenGameLevelEndedWinMenu() {
        _state = GameUIState.LevelEndedWinMenu;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(true);
        _levelEndedLoseMenuUI.SetActive(false);

        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(true);

        _levelSelectionUI.SetActive(false);
    }

    public void OpenGameLevelEndedLoseMenu() {
        _state = GameUIState.LevelEndedLoseMenu;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(true);

        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(true);

        _levelSelectionUI.SetActive(false);
    }

    public void OpenLevelSelectionMenu() {
        _state = GameUIState.LevelSelection;
        _mainMenuUI.SetActive(false);
        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);
        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(false);
        _levelSelectionUI.SetActive(true);
    }

    public void CloseAllUIMenus() {
        _mainMenuUI.SetActive(false);
        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);
        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(false);
        _levelSelectionUI.SetActive(false);
    }

    public void SetGameStateValuesUI(LevelInfo levelInfo) {

        _levelStartedLevelNameText.text = _levelStartedLevelNameTextFormat + levelInfo.GetLevelName();
        _levelStartedChapterNameText.text = _levelStartedChapterNameTextFormat + levelInfo.GetChapterName();
    }
    public void SetLevelStateDebugValuesUI(string packageToSpawn, string totalPackages, string packagesDestroyed, string packageDelivered, string numberOfPackageToWin) {
        _packageToSpawnText.text = 
            "Total packages: " + totalPackages + "\n" +
            "Number Of Package To Win: " + numberOfPackageToWin + "\n" +

            "Packages destroyed: " + packagesDestroyed + "\n" +
            "Packages delivered: " + packageDelivered + "\n" +


            "Packages to spawn: " + packageToSpawn + "\n";
    }

    private void BuildLevelListUI() {
        int levelNumb;
        GameObject chapterTitle;

        /* GENERATE CHATPER 1 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapterTitle.GetComponent<Text>().text = "Chapter 1";
        chapterTitle.transform.SetParent(_levelListUI.transform, false);

        levelNumb = GlobalPossibilityPath.GetChapterLevels(GlobalPossibilityPath.Chapter.Chapter1).Count;
        for(int i = 0; i < levelNumb; i++) {
            GameObject levelButton;
            levelButton = Instantiate(_buttonLevelUI);
            levelButton.transform.SetParent(_levelListUI.transform, false);
        }




        /* GENERATE CHATPER 2 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapterTitle.GetComponent<Text>().text = "Chapter 2";
        chapterTitle.transform.SetParent(_levelListUI.transform, false);

        levelNumb = GlobalPossibilityPath.GetChapterLevels(GlobalPossibilityPath.Chapter.Chapter2).Count;
        for(int i = 0; i < levelNumb; i++) {
            GameObject levelButton;
            levelButton = Instantiate(_buttonLevelUI);
            levelButton.transform.SetParent(_levelListUI.transform, false);
        }


        /* GENERATE CHATPER 3 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapterTitle.GetComponent<Text>().text = "Chapter 3";
        chapterTitle.transform.SetParent(_levelListUI.transform, false);

        levelNumb = GlobalPossibilityPath.GetChapterLevels(GlobalPossibilityPath.Chapter.Chapter3).Count;
        for(int i = 0; i < levelNumb; i++) {
            GameObject levelButton;
            levelButton = Instantiate(_buttonLevelUI);
            levelButton.transform.SetParent(_levelListUI.transform, false);
        }


        /* GENERATE CHATPER 4 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapterTitle.GetComponent<Text>().text = "Chapter 4";
        chapterTitle.transform.SetParent(_levelListUI.transform, false);

        levelNumb = GlobalPossibilityPath.GetChapterLevels(GlobalPossibilityPath.Chapter.Chapter4).Count;
        for(int i = 0; i < levelNumb; i++) {
            GameObject levelButton;
            levelButton = Instantiate(_buttonLevelUI);
            levelButton.transform.SetParent(_levelListUI.transform, false);
        }


        /* GENERATE CHATPER 5 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapterTitle.GetComponent<Text>().text = "Chapter 5";
        chapterTitle.transform.SetParent(_levelListUI.transform, false);

        levelNumb = GlobalPossibilityPath.GetChapterLevels(GlobalPossibilityPath.Chapter.Chapter5).Count;
        for(int i = 0; i < levelNumb; i++) {
            GameObject levelButton;
            levelButton = Instantiate(_buttonLevelUI);
            levelButton.transform.SetParent(_levelListUI.transform, false);
        }


        /* GENERATE CHATPER 6 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapterTitle.GetComponent<Text>().text = "Chapter 6";
        chapterTitle.transform.SetParent(_levelListUI.transform, false);

        levelNumb = GlobalPossibilityPath.GetChapterLevels(GlobalPossibilityPath.Chapter.Chapter6).Count;
        for(int i = 0; i < levelNumb; i++) {
            GameObject levelButton;
            levelButton = Instantiate(_buttonLevelUI);
            levelButton.transform.SetParent(_levelListUI.transform, false);
        }
    }

    public enum GameUIState {
        MainMenu,
        LevelStartedMenu,
        LevelEndedWinMenu,
        LevelEndedLoseMenu,
        PauseMenu,
        GameOverMenu,
        LevelSelection
    }
}
