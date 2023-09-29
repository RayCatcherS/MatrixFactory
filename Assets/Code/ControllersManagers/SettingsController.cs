using PT.Global;
using UnityEngine;

public class SettingsController : MonoBehaviour {

    [Header("Debug Settings")]
    [SerializeField] private bool _debugLowPerformance = false;
    public void InitSettings() {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        if(_debugLowPerformance) {
            Application.targetFrameRate = 4;
        }

    }

    public void SetGraphicSetting(int graphSet) {
        GameSaveManager.SetGraphicSettings(graphSet);
    }
    public void SaveGraphicSettings() {
        GameSaveManager.SaveGraphicSettings();
    }
}
