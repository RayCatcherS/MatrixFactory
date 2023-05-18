using UnityEngine;

public class ConveyorBeltInputReceiver : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;
    public void ConveyorClicked() {
        _conveyorBelt.ApplyPlatformConveyorRotation();
    }
}
