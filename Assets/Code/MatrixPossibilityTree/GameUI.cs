using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] private Image _background;
    private bool _isBackgroundEnable = true;
    private Color _targetColor;
    [SerializeField] private AnimationCurve _backgroundtransitionLerpCurve;
    private float _backgroundAnimationTransitionTime;
    private float _backgroundAnimationSpeed = 0.1f;

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
    
}
