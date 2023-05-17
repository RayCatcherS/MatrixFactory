using UnityEngine;

public class ConveyorBeltPlatform : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;
    

    public ConveyorPlatformMove GetConveyorBeltPlatformMove(Vector3 oldTargetPoint) {

        return _conveyorBelt.GetConveyorBeltPlatformMove(oldTargetPoint);
    }
}
