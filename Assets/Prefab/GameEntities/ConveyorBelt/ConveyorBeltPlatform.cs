using UnityEngine;

public class ConveyorBeltPlatform : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;


    public void ConveyorClicked() {
        _conveyorBelt.ApplyRollerConveyorRotation();
    }

    public ConveyorPlatformMove GetConveyorBeltPlatformMove(Vector3 oldTargetPoint) {

        return _conveyorBelt.GetConveyorBeltPlatformMove(oldTargetPoint);
    }
}
