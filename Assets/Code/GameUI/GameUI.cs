using JetBrains.Annotations;
using PT.DataStruct;
using PT.Global;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static PT.Global.GlobalPossibilityPath;

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
	[SerializeField] private GameObject _gameEndUI;
    [SerializeField] private GameObject _gameSettingsUI;

    [SerializeField] private GameObject _levelStartedMenuUI;
    [SerializeField] private GameObject _levelStartedGameStateUI;
    [SerializeField] private GameObject _incineratorInputButtonUI;
    
    


    [Header("Level UI")]
	[SerializeField] private Text _levelStartedLevelNameText;
    [SerializeField] private Text _levelStartedChapterNameText;
    [SerializeField] private Animator _sufficientPackagesIcon;
    

    [Header("Debug UI")]
	[SerializeField] private Text _packageToSpawnText;

    [Header("List Reference")]
    [SerializeField] private GameObject _levelListUI;

    [Header("Prefab References")]
    [SerializeField] private GameObject _buttonLevelUI;
    [SerializeField] private GameObject _textChapterTitleLevelUI;

    readonly private string _levelStartedLevelNameTextFormat = "Level: ";
    readonly private string _levelStartedChapterNameTextFormat = "Chapter: ";

    List<GameObject> _levelListElements = new List<GameObject>();

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
        CloseAndResetAllUIMenus();
        _mainMenuUI.SetActive(true);
	}

    public void OpenGameLevelStartedMenu(LevelInfo levelInfo) {
        _state = GameUIState.LevelStartedMenu;
        CloseAndResetAllUIMenus();
        _levelStartedMenuUI.SetActive(true);
        _levelStartedGameStateUI.SetActive(true);


        // disable incinerator button in the first level of the first chapter
        if(levelInfo.Chapter == Chapter.Chapter1 && levelInfo.LevelIndex == 0) {
            _incineratorInputButtonUI.SetActive(false);
        } else {
            _incineratorInputButtonUI.SetActive(true);
        }
    }

    public void OpenGameLevelWinMenu() {
        _state = GameUIState.LevelWinMenu; 
        CloseAndResetAllUIMenus();
        _levelEndedWinMenuUI.SetActive(true);
        _levelStartedGameStateUI.SetActive(true);
    }

    public void OpenGameLevelEndedLoseMenu() {
        _state = GameUIState.LevelLoseMenu;
        CloseAndResetAllUIMenus();
        _levelEndedLoseMenuUI.SetActive(true);
        _levelStartedGameStateUI.SetActive(true);
    }

    public void OpenLevelSelectionMenu() {
        _state = GameUIState.LevelSelection;
        CloseAndResetAllUIMenus();
        BuildLevelSelectionUI();
        _levelSelectionUI.SetActive(true);
    }

	public void OpenGameEnd() {
		_state = GameUIState.GameEnd;
        CloseAndResetAllUIMenus();
        _gameEndUI.SetActive(true);
	}

    public void OpenGameSettings() {
        _state = GameUIState.GameSettings;
        CloseAndResetAllUIMenus();

        _gameSettingsUI.SetActive(true);
    }

	public void CloseAndResetAllUIMenus() {
        _state = GameUIState.GameEnd;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);
        _gameEndUI.SetActive(false);

        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(false);

        _levelSelectionUI.SetActive(false);
        _gameSettingsUI.SetActive(false);

        gameObject.GetComponent<Tutorial>().CloseTutorial();

        changeIconSufficientPackages = false;
    }

    public void SetGameStateValuesUI(LevelInfo levelInfo) {

        _levelStartedLevelNameText.text = _levelStartedLevelNameTextFormat + levelInfo.GetLevelName();
        _levelStartedChapterNameText.text = _levelStartedChapterNameTextFormat + levelInfo.GetChapterName();
    }
    public void SetLevelStateDebugValuesUI(string packageToSpawn, string totalPackages, string packagesDestroyed, string packageDelivered, string numberOfPackageToWin) {
        /*_packageToSpawnText.text = 
            "Total packages: " + totalPackages + "\n" +
            "Number Of Package To Win: " + numberOfPackageToWin + "\n" +

            "Packages destroyed: " + packagesDestroyed + "\n" +
            "Packages delivered: " + packageDelivered + "\n" +


            "Packages to spawn: " + packageToSpawn + "\n";*/

        _packageToSpawnText.text = packageDelivered + "/" + numberOfPackageToWin;
    }

    private void BuildLevelSelectionUI() {
        int levelNumb;
        GameObject chapterTitle;
        GlobalPossibilityPath.Chapter chapter;

        DestroyLevelSelectionUI();

        /* GENERATE CHATPER 1 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapter = GlobalPossibilityPath.Chapter.Chapter1;
        chapterTitle.GetComponentInChildren<Text>().text = GlobalPossibilityPath.GetChapterString(chapter);
        chapterTitle.transform.SetParent(_levelListUI.transform, false);
        _levelListElements.Add(chapterTitle);
        levelNumb = GlobalPossibilityPath.GetChapterLevels(chapter).Count;
        for(int i = 0; i < levelNumb; i++) {
            IstantiateLevelButton(new LevelInfo(chapter, i));
        }




        /* GENERATE CHATPER 2 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapter = GlobalPossibilityPath.Chapter.Chapter2;
        chapterTitle.GetComponentInChildren<Text>().text = GlobalPossibilityPath.GetChapterString(chapter);
        chapterTitle.transform.SetParent(_levelListUI.transform, false);
        _levelListElements.Add(chapterTitle);
        levelNumb = GlobalPossibilityPath.GetChapterLevels(chapter).Count;
        for(int i = 0; i < levelNumb; i++) {
            IstantiateLevelButton(new LevelInfo(chapter, i));
        }


        /* GENERATE CHATPER 3 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapter = GlobalPossibilityPath.Chapter.Chapter3;
        chapterTitle.GetComponentInChildren<Text>().text = GlobalPossibilityPath.GetChapterString(chapter);
        chapterTitle.transform.SetParent(_levelListUI.transform, false);
        _levelListElements.Add(chapterTitle);
        levelNumb = GlobalPossibilityPath.GetChapterLevels(chapter).Count;
        for(int i = 0; i < levelNumb; i++) {
            IstantiateLevelButton(new LevelInfo(chapter, i));
        }


        /* GENERATE CHATPER 4 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapter = GlobalPossibilityPath.Chapter.Chapter4;
        chapterTitle.GetComponentInChildren<Text>().text = GlobalPossibilityPath.GetChapterString(chapter);
        chapterTitle.transform.SetParent(_levelListUI.transform, false);
        _levelListElements.Add(chapterTitle);
        levelNumb = GlobalPossibilityPath.GetChapterLevels(chapter).Count;
        for(int i = 0; i < levelNumb; i++) {
            IstantiateLevelButton(new LevelInfo(chapter, i));
        }


        /* GENERATE CHATPER 5 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapter = GlobalPossibilityPath.Chapter.Chapter5;
        chapterTitle.GetComponentInChildren<Text>().text = GlobalPossibilityPath.GetChapterString(chapter);
        chapterTitle.transform.SetParent(_levelListUI.transform, false);
        _levelListElements.Add(chapterTitle);
        levelNumb = GlobalPossibilityPath.GetChapterLevels(chapter).Count;
        for(int i = 0; i < levelNumb; i++) {
            IstantiateLevelButton(new LevelInfo(chapter, i));
        }


        /* GENERATE CHATPER 6 AND LEVELS */
        chapterTitle = Instantiate(_textChapterTitleLevelUI);
        chapter = GlobalPossibilityPath.Chapter.Chapter6;
        chapterTitle.GetComponentInChildren<Text>().text = GlobalPossibilityPath.GetChapterString(chapter);
        chapterTitle.transform.SetParent(_levelListUI.transform, false);
        _levelListElements.Add(chapterTitle);
        levelNumb = GlobalPossibilityPath.GetChapterLevels(chapter).Count;
        for(int i = 0; i < levelNumb; i++) {
            IstantiateLevelButton(new LevelInfo(chapter, i));
        }
    }

    private void IstantiateLevelButton(LevelInfo levelButtonInfo) {
        LevelInfo levelReached = GameSaveManager.GlobalLevelReached;


        GameObject levelButton;
        levelButton = Instantiate(_buttonLevelUI);
        levelButton.transform.SetParent(_levelListUI.transform, false);
        Text buttonText = levelButton.GetComponentInChildren<Text>();
        buttonText.text = "Level " + (levelButtonInfo.LevelIndex + 1);



        levelButton.GetComponent<Button>().onClick.AddListener(async () => {

            LevelInfo level = new LevelInfo(levelButtonInfo.Chapter, levelButtonInfo.LevelIndex);

            GameSaveManager.SetCurrentReachedLevel(level);

            await GameController.Instance.StartLevel(
                level
            );
        });
        

        if(GetChapterIndex(levelReached.Chapter) < GetChapterIndex(levelButtonInfo.Chapter)) {

            levelButton.GetComponent<GenericButton>().MakeButtonInteractable(false);
        } else if(GetChapterIndex(levelReached.Chapter) > GetChapterIndex(levelButtonInfo.Chapter)) {
            levelButton.GetComponent<GenericButton>().MakeButtonInteractable(true);
        } else if(GetChapterIndex(levelReached.Chapter) == GetChapterIndex(levelButtonInfo.Chapter)) {
            
            if(levelReached.LevelIndex >= levelButtonInfo.LevelIndex) {
                levelButton.GetComponent<GenericButton>().MakeButtonInteractable(true);
            } else {
                levelButton.GetComponent<GenericButton>().MakeButtonInteractable(false);
            }
        }

        _levelListElements.Add(levelButton);
    }

    private void DestroyLevelSelectionUI() {
        foreach(GameObject element in _levelListElements) {
            Destroy(element);
        }
        _levelListElements.Clear();
    }

    public enum GameUIState {
        MainMenu,
        LevelStartedMenu,
        LevelWinMenu,
        LevelLoseMenu,
        GameEnd,
        PauseMenu,
        GameoverMenu,
        LevelSelection,
        GameSettings
    }

    bool changeIconSufficientPackages = false;
    public void DrawSufficientPackages(bool value) {

        if (value != changeIconSufficientPackages) {
            changeIconSufficientPackages = value;
            if (value) {
                _sufficientPackagesIcon.ResetTrigger("complete");
                _sufficientPackagesIcon.ResetTrigger("notComplete");

                _sufficientPackagesIcon.SetTrigger("complete");
            } else {
                _sufficientPackagesIcon.ResetTrigger("complete");
                _sufficientPackagesIcon.ResetTrigger("notComplete");

                _sufficientPackagesIcon.SetTrigger("notComplete");
            }
        }
    }
}
