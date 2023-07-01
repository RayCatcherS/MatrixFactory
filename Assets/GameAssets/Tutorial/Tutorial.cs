using PT.DataStruct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    [Header("Tutorial UI References")]
    [SerializeField] private GameObject _tutorialUIScreen;
    [SerializeField] private GameObject _tutorialTitle;
    [SerializeField] private GameObject _tutorialCaption;
    [SerializeField] private GameObject _tutorialImage;
    [SerializeField] private GameObject _tutorialBackgroundImage;
    [SerializeField] private GameObject _tutorialNextButton;

    [Header("Tutorial Events")]
    [SerializeField] private List<TutorialEvent> _tutorialEvents = new List<TutorialEvent>();


    TutoriaInstance tutoriaInstance;

    void Start() {
        
    }
    public void CloseTutorial() {
        _tutorialUIScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Check if tutorial is needed for the level
    /// </summary>
    /// <param name="leveinfo">Level info for the check</param>
    /// <returns></returns>
    public async Task<bool> CheckTutorial(LevelInfo leveinfo) {

        bool tutorialRes = false;

        for(int i = 0;  i < _tutorialEvents.Count; i++) {

            if(_tutorialEvents[i].LevelTrigger.Chapter == leveinfo.Chapter && _tutorialEvents[i].LevelTrigger.LevelIndex == leveinfo.LevelIndex) {
                SelectTutorial(i);
                tutorialRes = true;
                break;
            }
        }

        if(tutorialRes) {
            await StartTutorial();
        }


        return tutorialRes;
    }

    private void SelectTutorial(int tutorialIndex) {
        tutoriaInstance = new TutoriaInstance(_tutorialEvents[tutorialIndex].TutorialPhases);
    }
    private async Task StartTutorial() {

        if (tutoriaInstance == null) {
            Debug.LogError("Tutorial instance is null");
            await Task.Yield();
            
        } else  {
            _tutorialUIScreen.gameObject.SetActive(true);
            DrawTutorialUI();
            while(!tutoriaInstance.TutorialEnded) {
                await Task.Yield();
            }
        }
        
        return;
    }

    public void NextTutorialPhase() {
        tutoriaInstance.IncreasePhaseIndex();

        if(tutoriaInstance == null) {
            Debug.LogError("Tutorial instance is null");

        } else if(tutoriaInstance.TutorialEnded) {
            CloseTutorial();
            
        } else {
            DrawTutorialUI();
        }
    } 

    private void DrawTutorialUI() {
        _tutorialTitle.GetComponent<Text>().text = tutoriaInstance.CurrentPhase.PhaseName;
        _tutorialCaption.GetComponent<Text>().text = tutoriaInstance.CurrentPhase.PhaseDescription;
        _tutorialImage.GetComponent<Image>().sprite = tutoriaInstance.CurrentPhase.PhaseImage;
        _tutorialBackgroundImage.GetComponent<Image>().sprite = tutoriaInstance.CurrentPhase.BackgroundImage;
    }
}

[Serializable]
class TutorialEvent {
    [SerializeField] private LevelInfoSerializable _levelTrigger;
    [SerializeField] private List<TutorialPhase> _tutorialPhases = new List<TutorialPhase>();

    public LevelInfoSerializable LevelTrigger {
        get { return _levelTrigger; }
    }
    public List<TutorialPhase> TutorialPhases {
        get { return _tutorialPhases; }
    }
}

[Serializable]
class TutorialPhase {
    [SerializeField] private string _phaseName;
    [SerializeField] private string _phaseDescription;
    [SerializeField] private Sprite _phaseImage;
    [SerializeField] private Sprite _backgroundImage;
    public string PhaseName {
        get { return _phaseName; }
    }
    public string PhaseDescription {
        get { return _phaseDescription; }
    }
    public Sprite PhaseImage {
        get { return _phaseImage; }
    }
    public Sprite BackgroundImage {
        get { return _backgroundImage; }
    }
}

class TutoriaInstance {

    private List<TutorialPhase> _tutorialPhases;
    public TutoriaInstance(List<TutorialPhase> tutorialPhases) {
        _tutorialPhases = tutorialPhases;
    }
    int _currentPhaseIndex = 0;

    public TutorialPhase CurrentPhase {
        get { return _tutorialPhases[_currentPhaseIndex]; }
    }
    public bool TutorialEnded {
        get { return _currentPhaseIndex >= _tutorialPhases.Count; }
    }

    public void IncreasePhaseIndex() {
        _currentPhaseIndex++;
    }
}
