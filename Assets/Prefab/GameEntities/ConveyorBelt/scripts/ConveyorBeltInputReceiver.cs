using UnityEngine;

public class ConveyorBeltInputReceiver : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;
    public void ConveyorClicked() {
        Debug.Log("ConveyorBeltInputReceiver.ConveyorClicked");
        _conveyorBelt.ApplyPlatformConveyorRotation();
    }
}
