using PT.Global;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static GameUI;

public class GameSettings : MonoBehaviour {

    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile lowProfile;
    [SerializeField] private VolumeProfile mediumHighProfile;
    private static int _selectedGraphicSetting = 2;

    private GameSettings _state;
    public GameSettings State => _state;
    public static GameSettings Instance { get; private set; }
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }


    public void SetGraphicSetting(int graphSet) {
        _selectedGraphicSetting = graphSet;
        
    }
    public void SaveGraphicSettings() {

        GameSaveManager.SaveGraphicSettings(_selectedGraphicSetting);
        InitGameSettings();
        //GameController.Instance.RestartGame();  // restart game to apply changes
    }

    public void InitGameSettings() {
        int qs = GameSaveManager.SavedGraphicSetting;
        Bloom bloom;
        volume.profile.TryGet<Bloom>(out bloom);

        if(qs == 0) {
            //volume.profile = lowProfile;
            bloom.active = false;
        } else {
            //volume.profile = mediumHighProfile;
            bloom.active = true;
        }
    }
}
