using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    [SerializeField] private Conveyor _conveyor;
    [SerializeField] private GameObject _debugTarget;

    private bool _initialized = false;
    private float _defaultConveyorHeight = 0f;
    private float _conveyorOffsetHeight = 0.5f;



    public float conveyorHeight {
        get { return _conveyor.gameObject.transform.position.y; }
    }

    public void InitConveyorBelt(double armConveyorHeight, Quaternion rotation) {
        if(_initialized) {
            Debug.LogError("the conveyor has already been initialized");
        }

        InitConveyorParameters();

        SetConveyorHeight(armConveyorHeight);
        SetConveyorRotation(rotation);

        SetDebugTarget(false);

        _initialized = true;
    }

    private void InitConveyorParameters() {
        _defaultConveyorHeight = _conveyor.gameObject.transform.position.y;
    }

    public void SetConveyorHeight(double height) {

        _conveyor.gameObject.transform.position = new Vector3(
            _conveyor.gameObject.transform.position.x,
            _defaultConveyorHeight + _conveyorOffsetHeight + (float)height,
            _conveyor.gameObject.transform.position.z
        );
    }

    private void SetConveyorRotation(Quaternion rotation) {
        _conveyor.gameObject.transform.rotation = rotation;
    }

    public void SetDebugTarget(bool value) {
        _debugTarget.SetActive(value);
    }

    public void RotateConveyor() {
        Debug.Log("Rotate Conveyor");
        _conveyor.gameObject.transform.rotation = Quaternion.Euler(
            _conveyor.gameObject.transform.eulerAngles.x,
            _conveyor.gameObject.transform.eulerAngles.y + 90,
            _conveyor.gameObject.transform.eulerAngles.z);
    }
}
