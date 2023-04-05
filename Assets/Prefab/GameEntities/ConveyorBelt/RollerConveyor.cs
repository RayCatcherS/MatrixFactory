using PT.DataStruct;
using UnityEngine;

public class RollerConveyor : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;


    public void ConveyorClicked() {
        _conveyorBelt.ApplyRollerConveyorRotation();
    }

    public Vector3 RollerConveyorDirectionVector() {

        return _conveyorBelt.RollerConveyorDirectionVector;
    }

    public float RollerConveyorSpeed() {
        return _conveyorBelt.RollerConveyorSpeed;
    }
}
