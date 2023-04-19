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
	readonly private int _rollerConveyorPlatformLayer = 3;
    readonly protected int _packageDamageColliderLayer = 7;
	readonly protected int _packageColliderLayer = 6;
	readonly protected int _deliveryPointCollider = 8;
	[SerializeField] private LayerMaskSelector _objectGroundToIgnore; // layer to ignore on fall


	[Header("Movement")]
	[SerializeField] private AnimationCurve _moveLerpCurve;
	[SerializeField] private AnimationCurve _fallLerpCurve;
    [SerializeField] private AnimationCurve _elevatorLerpCurve;
    [SerializeField] private float _objectFallSpeed = 3;
	[SerializeField] private float _objectSpeedMultiplyer = 1;
	private float _moveSpeed;
    private Vector3 _targetPoint;
	private Vector3 _startPoint;
	private float _animationTimePosition;
	private bool targetReached = true;
	private float targetReachedTollerance = 0.005f;
	private float _groundedTollerance = 0.04f;
	private TransportedObjMovementType objMovementType;



	protected Vector3 _objectSize;
    protected bool _objectInitialized = false;

	

	public void Init(Vector3 objectSize, LevelManager levelManager) {
		_objectSize = objectSize;
		_levelManager = levelManager;

		_moveSpeed = 0;
		_targetPoint = Vector3.zero;
        _startPoint = Vector3.zero;
		_animationTimePosition = 0;
		targetReached = true;

        _objectInitialized = true;
	}

	private void Update() {


		if (targetReached) {
			SetNextTarget();
		}
		else {
			ReachTargetPosition();
		}

	}

	private void ReachTargetPosition() {

		Vector3 distance = new Vector3(
			_targetPoint.x - transform.position.x,
			_targetPoint.y - transform.position.y,
			_targetPoint.z - transform.position.z
		);

		if(distance.magnitude > targetReachedTollerance) {


			if(objMovementType == TransportedObjMovementType.Fall) {

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
			else if(objMovementType == TransportedObjMovementType.Move) {

				transform.position = Vector3.Lerp(
					_startPoint,
					_targetPoint,
					_moveLerpCurve.Evaluate(_animationTimePosition)
				);

				_animationTimePosition += _moveSpeed * _objectSpeedMultiplyer * Time.deltaTime;
			} else if(objMovementType == TransportedObjMovementType.ElevatorCannon) {

                transform.position = Vector3.Lerp(
					_startPoint,
					_targetPoint,
                    _elevatorLerpCurve.Evaluate(_animationTimePosition)
				);
                _animationTimePosition += _moveSpeed * _objectSpeedMultiplyer * Time.deltaTime;
            }


		}
		else {
			targetReached = true;
		}
	}

	private void SetNextTarget() {
		_startPoint = transform.position;
		_animationTimePosition = 0;

		if (IsGrounded()) {

			ConfigureConveyorMoveToObject();
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

		objMovementType = TransportedObjMovementType.Fall;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, _objectGroundToIgnore.LayerMaskIgnore())) {

			_targetPoint = new Vector3(hit.point.x, hit.point.y + (_objectSize.y / 2), hit.point.z);
			targetReached = false;
		}
	}

	private void ConfigureConveyorMoveToObject() {

		// type of conveyor set the type of move
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, _objectGroundToIgnore.LayerMaskIgnore())) {
			
			if (hit.collider.gameObject.layer == _rollerConveyorPlatformLayer) {

				// get move info
				ConveyorBeltPlatform rollerConveyor = hit.collider.gameObject.GetComponent<ConveyorBeltPlatform>();
                ConveyorPlatformMove conveyorPlatformMove = rollerConveyor.GetConveyorBeltPlatformMove(_targetPoint);


				_targetPoint = conveyorPlatformMove.TargetPoint;
				objMovementType = conveyorPlatformMove.MovementType;
                _moveSpeed = conveyorPlatformMove.Speed;
                targetReached = false;
            }
		}
        
    }
}
