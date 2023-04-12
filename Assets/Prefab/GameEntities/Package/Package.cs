using UnityEngine;

public class Package : MonoBehaviour {
    private enum PackageMovementType {
        Move,
        Fall,
        ElevatorCannon
    }


    private LevelManager _levelManager;

    [Header("Layers")]
    readonly private int _rollerConveyorLayer = 3;
    readonly private int _packageDamageColliderLayer = 7;
    readonly private int _packageColliderLayer = 6;


    [Header("Movement")]
    [SerializeField] private AnimationCurve _moveLerpCurve;
    [SerializeField] private AnimationCurve _fallLerpCurve;
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


    private GameObject _packageDestroyedParticles;

    public void Init(Vector3 packageSize, GameObject packageDestroyedParticles, LevelManager levelManager) {
        _packageSize = packageSize;
        _packageDestroyedParticles = packageDestroyedParticles;
        _levelManager = levelManager;

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
                    _fallLerpCurve.Evaluate(_animationTimePosition)
                );

                _animationTimePosition += (_packageFallSpeed / unitDistance) * Time.deltaTime;

            } else if(packageMovementType == PackageMovementType.Move) {

                transform.position = Vector3.Lerp(
                    _startPoint,
                    _targetPoint,
                    _moveLerpCurve.Evaluate(_animationTimePosition)
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
                Vector3 direction = rollerConveyor.RollerConveyorDirectionVector();
                _targetPoint = _targetPoint + (direction);

                _packageRollerMoveSpeed = rollerConveyor.RollerConveyorSpeed();

                targetReached = false;
            }
        }
    }

    

    void OnCollisionEnter(Collision collision) {

        if(collision.gameObject.layer == _packageDamageColliderLayer || collision.gameObject.layer == _packageColliderLayer) {
            DestroyPackage();
        }
        
    }

    private void DestroyPackage() {
        gameObject.SetActive(false);

        GameObject particle = Instantiate(_packageDestroyedParticles, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        if(particleSystem != null) {
            particleSystem.Play();
        }

        _levelManager.PackageDestroyedEvent();
    }

    private void PackageDestinationReached() {
        Debug.Log("PackageDestinationReached");
    }
}
