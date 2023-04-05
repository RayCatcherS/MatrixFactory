using System.Collections;
using System.Collections.Generic;
using PT.DataStruct;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    private readonly int _CBrotationOffset = 90;
    readonly private float _conveyorOffsetHeight = 0.5f;
    private float _defaultConveyorHeight = 0f;

    [SerializeField] private RollerConveyor _rollerConveyor;
    [SerializeField] private GameObject _debugShowPath;
    [SerializeField] private float _rollerConveyorSpeed;


    private Quaternion _rollerConveyorRotationTarget = Quaternion.identity;
    [SerializeField] private float _rollerRotationSpeed = 2f;
    [SerializeField] private AnimationCurve _rollerRotationLerpCurve;
    private Direction _rollerConveyorDirection = Direction.stay;
    public Direction RollerConveyorDirection {
        get { return _rollerConveyorDirection; } 
    }
    public Vector3 RollerConveyorDirectionVector  {
        get {

            Vector3 value = Vector3.zero;

            if(_rollerConveyorDirection == Direction.forward) {
                value = new Vector3(-1, 0, 0);
            } else if(_rollerConveyorDirection == Direction.right) {
                value = new Vector3(0, 0, 1);
            } else if(_rollerConveyorDirection == Direction.back) {
                value = new Vector3(1, 0, 0);
            } else if(_rollerConveyorDirection == Direction.left) {
                value = new Vector3(0, 0, -1);
            }

            return value;
        }
        
    }

    private bool _initialized = false;

    public float RollerConveyorHeight {
        get { return _rollerConveyor.gameObject.transform.position.y; }
    }
    public float RollerConveyorSpeed {get { return _rollerConveyorSpeed; }}




    public void Update() {
        if(_initialized) {
            UpdateRollerConveyorRotation();
        }
        //_rollerConveyorRotationTarget
        //_rollerConveyor.gameObject.transform.rotation

    }

    public void InitConveyorBelt(double armConveyorHeight, Direction direction) {
        if(_initialized) {
            Debug.LogError("the conveyor has already been initialized");
        }

        InitConveyorParameters();

        SetRollerConveyorHeight(armConveyorHeight);
        SetRollerConveyorDirectionTarget(direction, false);

        EnableDebugShowPath(false);

        _initialized = true;
    }

    private void InitConveyorParameters() {
        _defaultConveyorHeight = _rollerConveyor.gameObject.transform.position.y;
    }

    public void SetRollerConveyorHeight(double height) {

        _rollerConveyor.gameObject.transform.position = new Vector3(
            _rollerConveyor.gameObject.transform.position.x,
            _defaultConveyorHeight + _conveyorOffsetHeight + (float)height,
            _rollerConveyor.gameObject.transform.position.z
        );
    }

    private void SetRollerConveyorDirectionTarget(Direction direction, bool animated) {

        Quaternion rotationVal = Quaternion.identity;

        if(direction == Direction.forward) {
            rotationVal = Quaternion.Euler(0, -_CBrotationOffset, 0);
            

        } else if(direction == Direction.right) {
            rotationVal = Quaternion.Euler(0, 0, 0);

        } else if(direction == Direction.back) {
            rotationVal = Quaternion.Euler(0, _CBrotationOffset, 0);
        } else if (direction == Direction.left) {
            rotationVal = Quaternion.Euler(0, _CBrotationOffset * 2, 0);
        }

        if(!animated) {
            _rollerConveyor.gameObject.transform.rotation = rotationVal;
        }
        _rollerConveyorRotationTarget = rotationVal;

        _rollerConveyorDirection = direction;
    }

    public void EnableDebugShowPath(bool value) {
        _debugShowPath.SetActive(value);
    }

    private Direction NextRollerConveyorDirection() {
        Direction nextDirection = Direction.stay;

        if(_rollerConveyorDirection == Direction.forward) {

            nextDirection = Direction.right;
        } else if(_rollerConveyorDirection == Direction.right) {
            nextDirection = Direction.back;
        } else if(_rollerConveyorDirection == Direction.back) {
            nextDirection = Direction.left;
        } else if(_rollerConveyorDirection == Direction.left) {
            nextDirection = Direction.forward;
        } 
        
        return nextDirection; 
        
    }
    public void ApplyRollerConveyorRotation() {
        Direction newDir = NextRollerConveyorDirection();

        SetRollerConveyorDirectionTarget(newDir, true);
    }

    private void UpdateRollerConveyorRotation() {
        if(_rollerConveyor.gameObject.transform.rotation != _rollerConveyorRotationTarget) {
            _rollerConveyor.gameObject.transform.rotation = Quaternion.Lerp(
                _rollerConveyor.gameObject.transform.rotation,
                _rollerConveyorRotationTarget,
                _rollerRotationLerpCurve.Evaluate(Time.deltaTime * _rollerRotationSpeed)
            );
        }
    }   
}
