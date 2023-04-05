using UnityEngine;

public class RollerConveyor : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;


    public void ConveyorClicked() {
        _conveyorBelt.RotateConveyor();
    }

    public Vector3 RollerConveyorDirection() {

        return transform.forward;
    }

    public Vector3 RollerConveyorTargetPos() {
        return transform.position;
    }

    public float RollerConveyorSpeed() {
        return _conveyorBelt.RollerConveyorSpeed;
    }
}
