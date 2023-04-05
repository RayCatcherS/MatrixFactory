using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour {
    public void SetSettings() {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
