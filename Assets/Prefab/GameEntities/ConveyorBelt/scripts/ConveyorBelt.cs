using PT.DataStruct;
using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

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
    [SerializeField] private GameObject _rollerPlatform;
    [SerializeField] private GameObject _elevatorPlatform;
    [SerializeField] private GameObject _incineratorPlatform;
    private GameObject _currentConveyorPlatform;
    private PlatformType _currentConveyorPlatformType;

    [Header("Base Types")]
    [SerializeField] private GameObject _baseRoller;
    [SerializeField] private GameObject _baseElevator;
    [SerializeField] private GameObject _baseIncinerator;

    [Header("Roller Platform")]
    [SerializeField] private GameObject _rollerPlatformMaterial;
    [Header("Roller Incinerator Platform")]
    [SerializeField] private GameObject _incineratorRollerPlatformRightMaterial;
    [SerializeField] private GameObject _incineratorRollerPlatformLeftMaterial;


    [Header("Debug")]
    [SerializeField] private GameObject _debugShowTruthPath;
    [Header ("Conveyor parameters")]
    [SerializeField] private float _rollerPlatformSpeed;
    [SerializeField] private float _elevatorCannonPlatformSpeed;
    private float _elevatorCannonTargetHeight;
    private Quaternion _platformRotationTarget = Quaternion.identity;
    [SerializeField] private float _platformRotationSpeed = 10;
    [SerializeField] private AnimationCurve _platformRotationLerpCurve;
    private Direction _platformDirection = Direction.stay;

    [Header("Conveyor triggers")]
    [SerializeField] private IncineratorTrigger _incineratorTrigger;

    [Header("References")]
    [SerializeField] private BoxCollider _platformInputCollider;
    [SerializeField] private BoxCollider _platformCollider;

    [Header("Conveyor rotation")]
    [SerializeField] private AudioClip _conveyorRotationClip;
    [SerializeField] private AudioClip _conveyorRollerClip;
    [SerializeField] private AudioClip _conveyorCannonClip;


    public Direction PlatformDirection {
        get { return _platformDirection; } 
    }
    private bool _initialized = false;

    public float PlatformConveyorHeight {
        get { return _currentConveyorPlatform.gameObject.transform.position.y; }
    }

    /// <summary>
    /// get the transported object target point and return the new target point
    /// </summary>
    /// <param name="oldTargetPoint"></param>
    /// <param name="_moveLerpCurve"></param>
    /// <param name="withConveyorAnimation">if true it starts a texture animation and a sound effect</param>
    /// <returns></returns>
    public ConveyorPlatformMove GetConveyorBeltPlatformMove(Vector3 oldTargetPoint, AnimationCurve _moveLerpCurve, bool withConveyorAnimation = true)  {
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

            if( withConveyorAnimation ) {
                StartRollerSound();
                StartCoroutine(MoveRollerTexture(_moveLerpCurve));
            }
            

        } else if(_currentConveyorPlatformType == PlatformType.ElevatorCannon) {


            move = new ConveyorPlatformMove(
                TransportedObject.TransportedObjMovementType.ElevatorCannon,
                new Vector3(oldTargetPoint.x, oldTargetPoint.y + _elevatorCannonTargetHeight, oldTargetPoint.z) + direction,
                _elevatorCannonPlatformSpeed
            );

            if(withConveyorAnimation) {
                StartCannonSound();
            }

        } else if(_currentConveyorPlatformType == PlatformType.Incinerator) {
            move = new ConveyorPlatformMove(
                TransportedObject.TransportedObjMovementType.Move,
                oldTargetPoint + direction,
                _rollerPlatformSpeed
            );

            if(withConveyorAnimation) {
                StartRollerSound();
                StartCoroutine(MoveIncineratorRollerTexture(_moveLerpCurve));
            }
        }

        return move;
    }
    
    private void StartRollerSound() {
        GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _conveyorRollerClip;
        audioSource.GetComponent<AudioSource>().Play();
    }
    private void StartCannonSound() {
        GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _conveyorCannonClip;
        audioSource.GetComponent<AudioSource>().Play();
    }
    IEnumerator MoveRollerTexture(AnimationCurve _moveLerpCurve) {

        float _animationTimePosition = 0;

        Material material = _rollerPlatformMaterial.GetComponent<Renderer>().material;
        Vector2 offsetTarget = new Vector2(material.mainTextureOffset.x - 0.4f, 0);

        while(material.mainTextureOffset.x > offsetTarget.x) {

            material.mainTextureOffset = Vector3.Lerp(material.mainTextureOffset, offsetTarget, _moveLerpCurve.Evaluate(_animationTimePosition));
            _animationTimePosition += (_rollerPlatformSpeed/2) * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveIncineratorRollerTexture(AnimationCurve _moveLerpCurve) {

        float _animationTimePosition = 0;

        Material materialRight = _incineratorRollerPlatformRightMaterial.GetComponent<Renderer>().material;
        Material materialLeft = _incineratorRollerPlatformLeftMaterial.GetComponent<Renderer>().material;

        Vector2 offsetTarget = new Vector2(materialRight.mainTextureOffset.x - 0.4f, 0);

        while(materialRight.mainTextureOffset.x > offsetTarget.x) {

            materialRight.mainTextureOffset = Vector3.Lerp(
                materialRight.mainTextureOffset,
                offsetTarget, _moveLerpCurve.Evaluate(_animationTimePosition)
            );
            materialLeft.mainTextureOffset = Vector3.Lerp(
                materialRight.mainTextureOffset,
                offsetTarget, _moveLerpCurve.Evaluate(_animationTimePosition)
            );
            _animationTimePosition += (_rollerPlatformSpeed / 2) * Time.deltaTime;
            yield return null;
        }
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

        SetPlatformConveyorHeight(conveyorPlatformHeight);
        SetPlatformDirection(platformDirection);
        ResetRollerPlatformMaterial();

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
        rotationSound();


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

    private Direction NextPlatformConveyorDirection() {
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
    public void ApplyPlatformConveyorRotation() {
        Direction newDir = NextPlatformConveyorDirection();

        SetPlatformDirectionTarget(newDir);
    }


    public void SetPlatformConveyorHeight(double height) {

        _currentConveyorPlatform.gameObject.transform.position = new Vector3(
            _currentConveyorPlatform.gameObject.transform.position.x,
            _defaultConveyorPlatformHeight + _conveyorOffsetHeight + (float)height,
            _currentConveyorPlatform.gameObject.transform.position.z
        );

        _platformCollider.gameObject.transform.position = new Vector3(
            _currentConveyorPlatform.gameObject.transform.position.x,
            _defaultConveyorPlatformHeight + _conveyorOffsetHeight + (float)height,
            _currentConveyorPlatform.gameObject.transform.position.z
        );

        _platformInputCollider.gameObject.transform.position = new Vector3(
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

            // rotate base
            if(_currentConveyorPlatformType == PlatformType.Incinerator) {
                _baseIncinerator.gameObject.transform.rotation = Quaternion.Lerp(
                    _currentConveyorPlatform.gameObject.transform.rotation,
                    _platformRotationTarget,
                    _platformRotationLerpCurve.Evaluate(Time.deltaTime * _platformRotationSpeed)
                );
            }
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
    public void SetElevatorCannonTargetHeight(float height) {
        _elevatorCannonTargetHeight = height;
    }

    private void ResetRollerPlatformMaterial() {
        Material material = _rollerPlatformMaterial.GetComponent<Renderer>().material;
        material.mainTextureOffset = Vector2.zero;
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

    private void rotationSound() {
        GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _conveyorRotationClip;
        audioSource.GetComponent<AudioSource>().Play();
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