
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private SettingsController _settingsController;
    [SerializeField] private LevelManager _levelManager;

    void Start() {
        _settingsController.SetSettings();

        _levelManager.InitLevel();
        _levelManager.StartLevel();


	}

    
}
