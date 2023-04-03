using UnityEngine;

public class Conveyor : MonoBehaviour {

    [SerializeField] private ConveyorBelt _conveyorBelt;

    void OnCollisionEnter(Collision collision) {
        
    }
    public void ConveyorClicked() {
        _conveyorBelt.RotateConveyor();
    }
}
