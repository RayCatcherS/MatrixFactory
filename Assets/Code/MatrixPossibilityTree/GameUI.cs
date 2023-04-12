using PT.DataStruct;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static PT.Global.GlobalPossibilityPath;

public class GameUI : MonoBehaviour {
    [SerializeField] private Image _background;
    private bool _isBackgroundEnable = true;
    private Color _targetColor;
    [SerializeField] private AnimationCurve _backgroundtransitionLerpCurve;
    private float _backgroundAnimationTransitionTime;
    private float _backgroundAnimationSpeed = 0.1f;

    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _levelEndedWinMenuUI;
    [SerializeField] private GameObject _levelEndedLoseMenuUI;

    [SerializeField] private GameObject _levelStartedMenuUI;
    [SerializeField] private GameObject _levelStartedGameStateUI;
    [SerializeField] private Text _levelStartedLevelNameText;
    [SerializeField] private Text _levelStartedChapterNameText;
    readonly private string _levelStartedLevelNameTextFormat = "Level: ";
    readonly private string _levelStartedChapterNameTextFormat = "Chapter: ";


    private GameUIState _state;
    public GameUIState State => _state;


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
    }

    public void OpenGameLevelStartedMenu() {
        _state = GameUIState.LevelStartedMenu;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);

        _levelStartedMenuUI.SetActive(true);
        _levelStartedGameStateUI.SetActive(true);
    }

    public void OpenGameLevelEndedWinMenu() {
        _state = GameUIState.LevelEndedWinMenu;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(true);
        _levelEndedLoseMenuUI.SetActive(false);

        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(true);
    }

    public void OpenGameLevelEndedLoseMenu() {
        _state = GameUIState.LevelEndedLoseMenu;
        _mainMenuUI.SetActive(false);

        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(true);

        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(true);
    }

    public void CloseAllUIMenus() {
        _mainMenuUI.SetActive(false);
        _levelEndedWinMenuUI.SetActive(false);
        _levelEndedLoseMenuUI.SetActive(false);
        _levelStartedMenuUI.SetActive(false);
        _levelStartedGameStateUI.SetActive(false);
    }

    public void SetGameStateValuesUI(LevelInfo levelInfo) {

        _levelStartedLevelNameText.text = _levelStartedLevelNameTextFormat + levelInfo.GetLevelName();
        _levelStartedChapterNameText.text = _levelStartedChapterNameTextFormat + levelInfo.GetChapterName();
    }

    public enum GameUIState {
        MainMenu,
        LevelStartedMenu,
        LevelEndedWinMenu,
        LevelEndedLoseMenu,
        PauseMenu,
        GameOverMenu
    }
}
