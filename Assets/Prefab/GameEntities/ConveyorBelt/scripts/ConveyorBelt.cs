using PT.DataStruct;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    public enum PlatformType {
        Roller,
        TrapDestroyer,
        ElevatorCannon,
        Incinerator,
        PackageDestroyer
    }

    private readonly int _CBrotationOffset = 90;
    readonly private float _conveyorOffsetHeight = 0.5f;
    private float _defaultConveyorPlatformHeight = 0f;

    [Header("Platform Types")]
    [SerializeField] private ConveyorBeltPlatform _rollerPlatform;
    [SerializeField] private ConveyorBeltPlatform _elevatorPlatform;
    [SerializeField] private ConveyorBeltPlatform _incineratorPlatform;
    private ConveyorBeltPlatform _currentConveyorPlatform;
    private PlatformType _currentConveyorPlatformType;

    [Header("Base Types")]
    [SerializeField] private GameObject _baseRoller;
    [SerializeField] private GameObject _baseElevator;
    [SerializeField] private GameObject _baseIncinerator;


    [Header("Debug")]
    [SerializeField] private GameObject _debugShowTruthPath;
    [Header ("Conveyor parameters")]
    [SerializeField] private float _rollerPlatformSpeed;
    [SerializeField] private float _elevatorCannonPlatformSpeed;
    private Quaternion _platformRotationTarget = Quaternion.identity;
    [SerializeField] private float _platformRotationSpeed = 10;
    [SerializeField] private AnimationCurve _platformRotationLerpCurve;
    private Direction _platformDirection = Direction.stay;

    [Header("Conveyor triggers")]
    [SerializeField] private IncineratorTrigger _incineratorTrigger;

    public Direction PlatformDirection {
        get { return _platformDirection; } 
    }

    /// <summary>
    /// Insert the transported object target point into the conveyor belt and get the new target point
    /// </summary>
    /// <param name="oldTargetPoint"></param>
    /// <returns></returns>
    public ConveyorPlatformMove GetConveyorBeltPlatformMove(Vector3 oldTargetPoint)  {
        ConveyorPlatformMove move = new ConveyorPlatformMove();

        Vector3 direction = Vector3.zero;
        if(_platformDirection == Direction.forward) {
            direction = new Vector3(-1, 0, 0);
        } else if(_platformDirection == Direction.right) {
            direction = new Vector3(0, 0, 1);
        } else if(_platformDirection == Direction.back) {
            direction = new Vector3(1, 0, 0);
        } else if(_platformDirection == Direction.left) {
            direction = new Vector3(0, 0, -1);
        }

        if(_currentConveyorPlatformType == PlatformType.Roller) {

            move = new ConveyorPlatformMove(
                TransportedObject.TransportedObjMovementType.Move,
                oldTargetPoint + direction,
                _rollerPlatformSpeed
            );


        } else if(_currentConveyorPlatformType == PlatformType.ElevatorCannon) {

            float elevatorCannonHeightForce = 1.5f;

            move = new ConveyorPlatformMove(
                TransportedObject.TransportedObjMovementType.ElevatorCannon,
                new Vector3(oldTargetPoint.x, oldTargetPoint.y + elevatorCannonHeightForce, oldTargetPoint.z) + direction,
                _elevatorCannonPlatformSpeed
            );

        } else if(_currentConveyorPlatformType == PlatformType.Incinerator) {
            move = new ConveyorPlatformMove(
                TransportedObject.TransportedObjMovementType.Move,
                oldTargetPoint + direction,
                _rollerPlatformSpeed
            );
        }

        return move;
    }

    private bool _initialized = false;

    public float RollerConveyorHeight {
        get { return _rollerPlatform.gameObject.transform.position.y; }
    }



    public void Update() {
        if(_initialized) {
            UpdateRollerConveyorRotation();
        }
    }

    public void InitConveyorBelt(double conveyorPlatformHeight, Direction platformDirection, PlatformType platformType) {
        if(_initialized) {
            Debug.LogError("the conveyor has already been initialized");
        }

        SetConveyorType(platformType);

        InitConveyorParameters();

        SetRollerConveyorHeight(conveyorPlatformHeight);
        SetPlatformDirection(platformDirection);

        EnableDebugShowPath(false);

        _initialized = true;
    }

    private void InitConveyorParameters() {
        _defaultConveyorPlatformHeight = _rollerPlatform.gameObject.transform.position.y;
    }

    /// <summary>
    /// Used to animate the rotation of the conveyor belt
    /// </summary>
    /// <param name="direction"></param>
    private void SetPlatformDirectionTarget(Direction direction) {

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
        _platformRotationTarget = rotationVal;

        _platformDirection = direction;
    }
    private void SetPlatformDirection(Direction direction) {
        Quaternion rotationVal = Quaternion.identity;

        if(direction == Direction.forward) {
            rotationVal = Quaternion.Euler(0, -_CBrotationOffset, 0);


        } else if(direction == Direction.right) {
            rotationVal = Quaternion.Euler(0, 0, 0);

        } else if(direction == Direction.back) {
            rotationVal = Quaternion.Euler(0, _CBrotationOffset, 0);
        } else if(direction == Direction.left) {
            rotationVal = Quaternion.Euler(0, _CBrotationOffset * 2, 0);
        }
        _rollerPlatform.gameObject.transform.rotation = rotationVal;
        _incineratorPlatform.gameObject.transform.rotation = rotationVal;
        _elevatorPlatform.gameObject.transform.rotation = rotationVal;
        _platformRotationTarget = rotationVal;
        _platformDirection = direction;
    }

    public void EnableDebugShowPath(bool value) {
        _debugShowTruthPath.SetActive(value);
        _debugShowTruthPath.transform.position = new Vector3(
            _currentConveyorPlatform.gameObject.transform.position.x,
            _currentConveyorPlatform.gameObject.transform.position.y + 1,
            _currentConveyorPlatform.gameObject.transform.position.z
        );
    }

    private Direction NextRollerConveyorDirection() {
        Direction nextDirection = Direction.stay;

        if(_platformDirection == Direction.forward) {

            nextDirection = Direction.right;
        } else if(_platformDirection == Direction.right) {
            nextDirection = Direction.back;
        } else if(_platformDirection == Direction.back) {
            nextDirection = Direction.left;
        } else if(_platformDirection == Direction.left) {
            nextDirection = Direction.forward;
        } 
        
        return nextDirection; 
        
    }
    public void ApplyRollerConveyorRotation() {
        Direction newDir = NextRollerConveyorDirection();

        SetPlatformDirectionTarget(newDir);
    }


    public void SetRollerConveyorHeight(double height) {

        _currentConveyorPlatform.gameObject.transform.position = new Vector3(
            _currentConveyorPlatform.gameObject.transform.position.x,
            _defaultConveyorPlatformHeight + _conveyorOffsetHeight + (float)height,
            _currentConveyorPlatform.gameObject.transform.position.z
        );
    }
    private void UpdateRollerConveyorRotation() {
        if(_currentConveyorPlatform.gameObject.transform.rotation != _platformRotationTarget) {
            _currentConveyorPlatform.gameObject.transform.rotation = Quaternion.Lerp(
                _currentConveyorPlatform.gameObject.transform.rotation,
                _platformRotationTarget,
                _platformRotationLerpCurve.Evaluate(Time.deltaTime * _platformRotationSpeed)
            );
        }
    }

    public void SetConveyorType(PlatformType conveyorBeltType) {

        if(conveyorBeltType == PlatformType.Roller) {
            _rollerPlatform.gameObject.SetActive(true);
            _elevatorPlatform.gameObject.SetActive(false);
            _incineratorPlatform.gameObject.SetActive(false);

            _baseRoller.SetActive(true);
            _baseElevator.SetActive(false);
            _baseIncinerator.SetActive(false);

            _currentConveyorPlatform = _rollerPlatform;

        } else if(conveyorBeltType == PlatformType.TrapDestroyer) {
            _rollerPlatform.gameObject.SetActive(false);
            _elevatorPlatform.gameObject.SetActive(false);
            _incineratorPlatform.gameObject.SetActive(false);

            _baseRoller.SetActive(false);
            _baseElevator.SetActive(false);
            _baseIncinerator.SetActive(false);

            _currentConveyorPlatform = null;

        } else if(conveyorBeltType == PlatformType.ElevatorCannon) {
            _rollerPlatform.gameObject.SetActive(false);
            _elevatorPlatform.gameObject.SetActive(true);
            _incineratorPlatform.gameObject.SetActive(false);

            _baseRoller.SetActive(false);
            _baseElevator.SetActive(true);
            _baseIncinerator.SetActive(false);

            _currentConveyorPlatform = _elevatorPlatform;
            

        } else if(conveyorBeltType == PlatformType.Incinerator) {
            _rollerPlatform.gameObject.SetActive(false);
            _elevatorPlatform.gameObject.SetActive(false);
            _incineratorPlatform.gameObject.SetActive(true);

            _baseRoller.SetActive(false);
            _baseElevator.SetActive(false);
            _baseIncinerator.SetActive(true);

            _currentConveyorPlatform = _incineratorPlatform;
        }
        _currentConveyorPlatformType = conveyorBeltType;
    }

    public void SetEnableIncineratorPlatformTrigger(bool value) {
        if(_currentConveyorPlatformType == PlatformType.Incinerator) {

            if(value) {
                _incineratorTrigger.OpenIncinerator();
            } else {
                _incineratorTrigger.CloseIncinerator();
            }
            
        } else {
            throw new System.InvalidOperationException("The conveyorBelt is not an incinerator conveyor belt");
        }

    }
}

public class ConveyorPlatformMove {

    private readonly TransportedObject.TransportedObjMovementType _movementType;
    private readonly Vector3 _targetPoint;
    private readonly float _speed;

    public ConveyorPlatformMove() { }
    public ConveyorPlatformMove(TransportedObject.TransportedObjMovementType movementType, Vector3 targetPoint, float speed) {
        _movementType = movementType;
        _targetPoint = targetPoint;
        _speed = speed;
    }

    public TransportedObject.TransportedObjMovementType MovementType {
        get { return _movementType; }
    }
    public Vector3 TargetPoint { get { return _targetPoint; } }
    public float Speed { get { return _speed;} }
}