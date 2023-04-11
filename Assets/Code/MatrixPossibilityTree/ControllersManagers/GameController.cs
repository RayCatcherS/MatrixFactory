
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private SettingsController _settingsController;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameUI _gameUI;

    void Start() {

        StartGameAsync();
        


    }

    private async Task StartGameAsync() {

        _settingsController.SetSettings();
        _levelManager.InitLevel();
        


        await _gameUI.SetBlackBackgroundLerp(false);



        _levelManager.StartLevel();

        
    }
}
