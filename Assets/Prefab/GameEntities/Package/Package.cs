using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Package : MonoBehaviour {
    readonly private int _rollerConveyorLayer = 3;


    [SerializeField] private AnimationCurve _moveAnimationCurve;
    [SerializeField] private AnimationCurve _fallAnimationCurve;

    [SerializeField] private float _packageFallSpeed = 3;
    private float _packageRollerMoveSpeed;

    private Vector3 _targetPoint;
    private Vector3 _startPoint;
    private float _animationTimePosition;
    private bool targetReached = true;
    private float targetReachedTollerance = 0.005f;

    private float _groundedTollerance = 0.04f;
    private PackageMovementType packageMovementType;

    private Vector3 _packageSize;
    private bool _packageInitialized = false;

    public void Init(Vector3 packageSize) {
        _packageSize = packageSize;

        _packageInitialized = true;

    }


    private void Start() {

        if(!_packageInitialized) {
            Debug.LogError("Package has not been initialized");
        }
    }

    private void Update() {


        if(targetReached) {
            SetNextTarget();
        } else {
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


            if(packageMovementType == PackageMovementType.Fall) {

                Vector3 fallDistance = new Vector3(
                    _targetPoint.x - _startPoint.x,
                    _targetPoint.y - _startPoint.y,
                    _targetPoint.z - _startPoint.z
                );
                float unitDistance = fallDistance.magnitude;

                transform.position = Vector3.Lerp(
                    _startPoint,
                    _targetPoint,
                    _fallAnimationCurve.Evaluate(_animationTimePosition)
                );

                _animationTimePosition += (_packageFallSpeed / unitDistance) * Time.deltaTime;

            } else if(packageMovementType == PackageMovementType.Move) {

                transform.position = Vector3.Lerp(
                    _startPoint,
                    _targetPoint,
                    _moveAnimationCurve.Evaluate(_animationTimePosition)
                );

                _animationTimePosition += _packageRollerMoveSpeed * Time.deltaTime;
            }


        } else {
            targetReached = true;
        }
    }

    private void SetNextTarget() {
        _startPoint = transform.position;
        _animationTimePosition = 0;

        if(IsGrounded()) {

            SetConveyorDirectionAsTarget();
        } else {
            SetFloorAsTarget();
        }
    }

    private bool IsGrounded() {
        bool value;

        value = Physics.Raycast(transform.position, -Vector3.up, _groundedTollerance + (_packageSize.y / 2));
        Debug.DrawRay(transform.position, -Vector3.up * (_groundedTollerance + (_packageSize.y / 2)), Color.green);


        return value;
    }

    private void SetFloorAsTarget() {

        packageMovementType = PackageMovementType.Fall;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity)) {

            _targetPoint = new Vector3(hit.point.x, hit.point.y + (_packageSize.y / 2), hit.point.z);
            targetReached = false;
        }
    }

    private void SetConveyorDirectionAsTarget() {

        // type of conveyor set the type of muve
        // the normal roller conveyor is linear, and the elevator has an another movement
        packageMovementType = PackageMovementType.Move;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity)) {
            if(hit.collider.gameObject.layer == _rollerConveyorLayer) {

                // set new target
                RollerConveyor rollerConveyor = hit.collider.gameObject.GetComponent<RollerConveyor>();
                Vector3 direction = rollerConveyor.RollerConveyorDirection();
                _targetPoint = _targetPoint + (direction);

                _packageRollerMoveSpeed = rollerConveyor.RollerConveyorSpeed();

                targetReached = false;
            }
        }
    }

    private enum PackageMovementType {
        Move,
        Fall,
        ElevatorCannon
    }
}
