using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    [SerializeField] private GameObject _conveyor;
    [SerializeField] private GameObject _debugTarget;

    private bool initialized = false;
    private float defaultConveyorHeight = 0f;
    private float conveyorOffsetHeight = 0.5f;



    public float conveyorHeight {
        get { return _conveyor.gameObject.transform.position.y; }
    }

    public void InitConveyorBelt(double armConveyorHeight, Quaternion rotation) {
        if(initialized) {
            Debug.LogError("the conveyor has already been initialized");
        }

        InitConveyorParameters();

        SetConveyorHeight(armConveyorHeight);
        SetConveyorRotation(rotation);

        SetDebugTarget(false);

        initialized = true;
    }

    private void InitConveyorParameters() {
        defaultConveyorHeight = _conveyor.gameObject.transform.position.y;
    }

    public void SetConveyorHeight(double height) {

        _conveyor.gameObject.transform.position = new Vector3(
            _conveyor.gameObject.transform.position.x,
            defaultConveyorHeight + conveyorOffsetHeight + (float) height,
            _conveyor.gameObject.transform.position.z
        );
    }

    private void SetConveyorRotation(Quaternion rotation) {
        _conveyor.gameObject.transform.rotation = rotation;
    }

    public void SetDebugTarget(bool value) {
        _debugTarget.SetActive(value);
    }
}
