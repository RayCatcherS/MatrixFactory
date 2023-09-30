using System.Threading.Tasks;
using UnityEngine;

public class GameSignal : MonoBehaviour {

    private bool _signalStarted = false;

    public void StartSignal() {
        gameObject.GetComponent<Animation>().enabled = true;
    }

    public void StopSignal() {
        gameObject.GetComponent<Animation>().enabled = false;
    }

}
