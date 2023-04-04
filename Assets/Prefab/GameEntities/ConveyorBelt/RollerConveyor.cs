using UnityEngine;

public class RollerConveyor : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;


    public void ConveyorClicked() {
        _conveyorBelt.RotateConveyor();
    }

    public Vector3 GetRollerConveyorDirection() {

        return transform.forward;
    }

    public Vector3 GetRollerConveyorTargetPos() {
        return transform.position;
    }
}
