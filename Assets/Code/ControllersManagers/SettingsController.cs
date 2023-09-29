using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour {
    public void InitSettings() {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        //Application.targetFrameRate = 4;

    }
}
