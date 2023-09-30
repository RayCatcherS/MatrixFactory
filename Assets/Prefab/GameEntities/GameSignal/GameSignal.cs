using System.Threading.Tasks;
using UnityEngine;

public class GameSignal : MonoBehaviour {

    [SerializeField] private GameObject _light;
    private bool _signalStarted = false;

    public void StartSignal() {
        gameObject.GetComponent<Animation>().enabled = true;
    }

    public void StopSignal() {
        _light.SetActive(false);
        gameObject.GetComponent<Animation>().enabled = false;
    }

}
