using System.Collections.Generic;
using UnityEngine;

public class TransportedObject : MonoBehaviour {

	public enum TransportedObjMovementType {
		Move,
		Fall,
		ElevatorCannon
	}


	protected LevelManager _levelManager;

	[Header("Layers")]
	readonly protected int _rollerConveyorPlatformLayer = 3;
    readonly protected int _packageDamageColliderLayer = 7;
    readonly protected int _physicsPackageDamageColliderLayer = 13;
    readonly protected int _physicsPackageLayer = 11;
    readonly protected int _packageColliderLayer = 6;
	readonly protected int _deliveryPointCollider = 8;
	[SerializeField] protected LayerMaskSelector _objectGroundToIgnore; // layer to ignore on fall


	[Header("Movement")]
	[SerializeField] private AnimationCurve _moveLerpCurve;
	[SerializeField] private AnimationCurve _fallLerpCurve;
    [SerializeField] private AnimationCurve _elevatorVerticalAxisLerpCurve;
    [SerializeField] private AnimationCurve _elevatorHorizontalAxisLerpCurve;
    [SerializeField] protected float _objectFallSpeed = 3;
	[SerializeField] private float _objectSpeedMultiplyer = 1;
	[SerializeField] private bool _animateRollerOnGettingMove;
	private float _moveSpeed;
    protected Vector3 _targetPoint;
	private Vector3 _startPoint;
	private float _animationTimePosition;
	private bool _targetReached = true;
	private float _targetReachedTollerance = 0.005f;
	private float _groundedTollerance = 0.04f;
	private TransportedObjMovementType _objMovementType = TransportedObjMovementType.Fall;



	protected Vector3 _objectSize;
    protected bool _objectInitialized = false;
	protected float _objectVelocity = 0;
	private Vector3 _lastObjectPosition;



    public void Init(Vector3 objectSize, LevelManager levelManager) {
		_objectSize = objectSize;
		_levelManager = levelManager;
        ResetObject();

    }

    public void ResetObject() {
        _moveSpeed = 0;
		_targetPoint = Vector3.zero;
        _startPoint = Vector3.zero;
        _animationTimePosition = 0;
        _targetReached = true;

        _objectInitialized = true;
    }

    private void FixedUpdate() {
		if (_targetReached) {
			SetNextTarget();
		} else {
			ReachTargetPositionUpdate();
		}

	}

	private void ReachTargetPositionUpdate() {

		Vector3 distance = new Vector3(
			_targetPoint.x - transform.position.x,
			_targetPoint.y - transform.position.y,
			_targetPoint.z - transform.position.z
		);

		if(distance.magnitude > _targetReachedTollerance) {

			if(_objMovementType == TransportedObjMovementType.Fall) {
				FallMoveUpdate();

			} else if(_objMovementType == TransportedObjMovementType.Move) {
				MoveUpdate();

			} else if(_objMovementType == TransportedObjMovementType.ElevatorCannon) {
                ElevatorCannonMoveUpdate();
            }
		}
		else {
			_targetReached = true;
		}
	}


	private void FallMoveUpdate() {
        Vector3 fallDistance = new Vector3(
                    _targetPoint.x - _startPoint.x,
                    _targetPoint.y - _startPoint.y,
                    _targetPoint.z - _startPoint.z
                );
        float unitDistance = fallDistance.magnitude;

        transform.position = Vector3.Lerp(
            _startPoint,
            _targetPoint,
            _fallLerpCurve.Evaluate(_animationTimePosition)
        );
        // increase in speed in proportion to the distance to be covered
        _animationTimePosition += (_objectFallSpeed / unitDistance) * _objectSpeedMultiplyer * Time.deltaTime;
    }
	private void MoveUpdate() {
        _lastObjectPosition = transform.position;
        transform.position = Vector3.Lerp(
                    _startPoint,
                    _targetPoint,
                    _moveLerpCurve.Evaluate(_animationTimePosition)
                );

        _animationTimePosition += _moveSpeed * _objectSpeedMultiplyer * Time.deltaTime;
		_objectVelocity = (transform.position - _lastObjectPosition).magnitude / Time.deltaTime;
    }
	private void ElevatorCannonMoveUpdate() {

		transform.position = new Vector3(
            Mathf.Lerp(_startPoint.x, _targetPoint.x, _elevatorHorizontalAxisLerpCurve.Evaluate(_animationTimePosition)),
            Mathf.Lerp(_startPoint.y, _targetPoint.y, _elevatorVerticalAxisLerpCurve.Evaluate(_animationTimePosition)),
            Mathf.Lerp(_startPoint.z, _targetPoint.z, _elevatorHorizontalAxisLerpCurve.Evaluate(_animationTimePosition))
        );

        /*transform.position = Vector3.Lerp(
			_startPoint,
            _targetPoint,
            _elevatorVerticalAxisLerpCurve.Evaluate(_animationTimePosition)
        );*/

        _animationTimePosition += _moveSpeed * Time.deltaTime;
    }


	private bool firstTargetSetted = false;
	private float lastTime = 0;

	private void SetNextTarget() {
        

		_startPoint = transform.position;
		_animationTimePosition = 0;

		if (IsGrounded()) {
			GetMoveFromConveyorplatform();
		} else {
			SetFloorAsTarget();
		}
	}

	private bool IsGrounded() {
		bool value;

		value = Physics.Raycast(
			 transform.position,
			   -Vector3.up,
			   _groundedTollerance + (_objectSize.y / 2),
			   _objectGroundToIgnore.LayerMaskIgnore()
		);

		Debug.DrawRay(transform.position, -Vector3.up * (_groundedTollerance + (_objectSize.y / 2)), Color.green);


		return value;
	}

	
	private void SetFloorAsTarget() {

		_objMovementType = TransportedObjMovementType.Fall;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, _objectGroundToIgnore.LayerMaskIgnore())) {

			_targetPoint = new Vector3(hit.point.x, hit.point.y + (_objectSize.y / 2), hit.point.z);
			_targetReached = false;
		}
	}

	private void GetMoveFromConveyorplatform() {

		// type of conveyor set the type of move
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, _objectGroundToIgnore.LayerMaskIgnore())) {
			
			if (hit.collider.gameObject.layer == _rollerConveyorPlatformLayer) {

				// get move info
				ConveyorBeltPlatform rollerConveyor = hit.collider.gameObject.GetComponent<ConveyorBeltPlatform>();
                ConveyorPlatformMove conveyorPlatformMove = rollerConveyor.GetConveyorBeltPlatformMove(
					_targetPoint,
					_moveLerpCurve,
					_animateRollerOnGettingMove
				);
				_targetPoint = conveyorPlatformMove.TargetPoint;
				_objMovementType = conveyorPlatformMove.MovementType;
                _moveSpeed = conveyorPlatformMove.Speed;
                _targetReached = false;
            }
		}
        
    }

    
}
