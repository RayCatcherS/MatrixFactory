
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    private SettingsController _settingsController;

    public CameraController cameraController {
        get { return _cameraController; }
    }

    void Start() {
        InitComponent();

        _settingsController.SetSettings();
    }

    private void InitComponent() {
        _settingsController = gameObject.GetComponent<SettingsController>();
    }
}
