using System.Collections;
using System.Collections.Generic;
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
}
