using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameSignal : MonoBehaviour {

    [SerializeField] private AnimationClip flickering;
    [SerializeField] private AnimationClip offLight;
    private bool _signalStarted = false;

    
    public void StartSignal() {
        if (_signalStarted) return;
        //RecursiveLight();
        gameObject.GetComponent<Animation>().clip = flickering;
        gameObject.GetComponent<Animation>().Play();

        _signalStarted = true;
    }

    public void StopSignal() {
        gameObject.GetComponent<Animation>().clip = offLight;
        gameObject.GetComponent<Animation>().Play();
        _signalStarted = false;
    }

    /*async private void RecursiveLight() {
        while(true) {

            light.gameObject.SetActive(true);
            await Task.Delay((int)(millisecondWaitTime / 2f));

            light.gameObject.SetActive(false);
            await Task.Delay((int)(millisecondWaitTime / 2f));

            Debug.LogWarning("A Debug.Warning() call");
            if(!_signalStarted) return;
        }
    }*/
}
