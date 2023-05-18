using System.Threading.Tasks;
using UnityEngine;

public class GameSignal : MonoBehaviour {

    [SerializeField] private GameObject light;
    [SerializeField] private float millisecondWaitTime = 1000f;
    private bool _signalStarted = false;

    public void StartSignal() {
        if (_signalStarted) return;
        RecursiveLight();
        _signalStarted = true;
    }

    public void StopSignal() {
        light.gameObject.SetActive(false);
        _signalStarted = false;
    }

    async private void RecursiveLight() {
        while(true) {

            light.gameObject.SetActive(true);
            await Task.Delay((int)(millisecondWaitTime / 2f));

            light.gameObject.SetActive(false);
            await Task.Delay((int)(millisecondWaitTime / 2f));

            if(!_signalStarted) return;
        }
    }
}
