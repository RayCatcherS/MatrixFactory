using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    [SerializeField] private GameObject _conveyor;

    private bool initialized = false;
    private float defaultConveyorHeight = 0f;

    public void InitConveyorBelt(double armConveyorHeight, Quaternion rotation) {
        if(initialized) {
            Debug.LogError("the conveyor has already been initialized");
        }

        InitConveyorParameters();

        SetConveyorHeight(armConveyorHeight);
        SetConveyorRotation(rotation);

        initialized = true;
    }

    private void InitConveyorParameters() {
        defaultConveyorHeight = _conveyor.gameObject.transform.position.y;
    }

    public void SetConveyorHeight(double height) {

        _conveyor.gameObject.transform.position = new Vector3(
            _conveyor.gameObject.transform.position.x,
            defaultConveyorHeight + (float) height,
            _conveyor.gameObject.transform.position.z
        );
    }

    private void SetConveyorRotation(Quaternion rotation) {
        _conveyor.gameObject.transform.rotation = rotation;
    }
}
