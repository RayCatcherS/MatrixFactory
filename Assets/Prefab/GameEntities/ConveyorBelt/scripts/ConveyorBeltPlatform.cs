using UnityEngine;

public class ConveyorBeltPlatform : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;
    

    public ConveyorPlatformMove GetConveyorBeltPlatformMove(Vector3 oldTargetPoint, AnimationCurve _moveLerpCurve, bool withConveyorAnimation = true) {

        return _conveyorBelt.GetConveyorBeltPlatformMove(oldTargetPoint, _moveLerpCurve, withConveyorAnimation);
    }
}
