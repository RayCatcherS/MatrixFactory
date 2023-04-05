using System.Collections;
using System.Collections.Generic;
using PT.DataStruct;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    private readonly int _CBrotationOffset = 90;


    [SerializeField] private RollerConveyor _rollerConveyor;
    [SerializeField] private GameObject _debugTarget;
    [SerializeField] private float _rollerConveyorSpeed;

    private bool _initialized = false;
    private float _defaultConveyorHeight = 0f;
    private float _conveyorOffsetHeight = 0.5f;



    public float RollerConveyorHeight {
        get { return _rollerConveyor.gameObject.transform.position.y; }
    }

    public float RollerConveyorSpeed {
        get { return _rollerConveyorSpeed; }
    }

    public void InitConveyorBelt(double armConveyorHeight, Direction direction) {
        if(_initialized) {
            Debug.LogError("the conveyor has already been initialized");
        }

        InitConveyorParameters();

        SetConveyorHeight(armConveyorHeight);
        SetConveyorDirectionTarget(direction);

        SetDebugTarget(false);

        _initialized = true;
    }

    private void InitConveyorParameters() {
        _defaultConveyorHeight = _rollerConveyor.gameObject.transform.position.y;
    }

    public void SetConveyorHeight(double height) {

        _rollerConveyor.gameObject.transform.position = new Vector3(
            _rollerConveyor.gameObject.transform.position.x,
            _defaultConveyorHeight + _conveyorOffsetHeight + (float)height,
            _rollerConveyor.gameObject.transform.position.z
        );
    }

    private void SetConveyorDirectionTarget(Direction direction) {


        if(direction == Direction.forward) {
            _rollerConveyor.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        } else if(direction == Direction.right) {
            _rollerConveyor.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if(direction == Direction.back) {
            _rollerConveyor.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        } else if (direction == Direction.left) {
            _rollerConveyor.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //_conveyor.gameObject.transform.rotation = rotation;
    }

    public void SetDebugTarget(bool value) {
        _debugTarget.SetActive(value);
    }

    public void RotateConveyor() {

        _rollerConveyor.gameObject.transform.rotation = Quaternion.Euler(
            _rollerConveyor.gameObject.transform.eulerAngles.x,
            _rollerConveyor.gameObject.transform.eulerAngles.y + 90,
            _rollerConveyor.gameObject.transform.eulerAngles.z);
    }
}
