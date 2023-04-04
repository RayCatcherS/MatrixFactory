using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsController : MonoBehaviour {

    [SerializeField] private CameraController _cameraController;
    //[SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private Camera _camera;
    private CinemachinePOV _pov;


    [SerializeField] private float _XrotationSpeed = 5;
    [SerializeField] private float _YrotationSpeed = 5;
    [SerializeField] private float _minY = 35;
    [SerializeField] private float _maxY = 70;
    private bool _dragRotationActive = false;

    private Vector2 _lateMousePosition = Vector2.zero;
    private Vector2 _mouseMovementDelta = Vector2.zero;


    private float _lerpValue = 0;
    private float _tapInteractionTimer = 0;
    readonly private int _conveyorRollerLayer = 3;


    void Start() {
        InitGameComponents();
        InitCinemachine();
    }


    void Update() {

        HandleCameraRotation();
        TapInteraction();
    }

    private void InitGameComponents() {
        _camera = _cameraController.gameObject.GetComponent<Camera>();
    }
    private void InitCinemachine() {
        // cinemachine virtual camera
        _pov = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    
    private void HandleCameraRotation() {

        if(Input.GetMouseButtonDown(0)) {

            _dragRotationActive = true;
            _lateMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0)) {

            _dragRotationActive = false;
        }

        if(_dragRotationActive) {
            _mouseMovementDelta = (Vector2)Input.mousePosition - _lateMousePosition;
            _lerpValue = 0f;

            _lateMousePosition = Input.mousePosition;
        }

        float rotationXValue = _XrotationSpeed * Time.deltaTime * _mouseMovementDelta.x;
        float rotationYValue = _YrotationSpeed * Time.deltaTime * _mouseMovementDelta.y;





        _pov.m_HorizontalAxis.Value = _pov.m_HorizontalAxis.Value + rotationXValue;
        _pov.m_VerticalAxis.Value = Mathf.Clamp(_pov.m_VerticalAxis.Value - rotationYValue, _minY, _maxY);



        _mouseMovementDelta = new Vector2(
            Mathf.Lerp(_mouseMovementDelta.x, 0, _lerpValue),
            Mathf.Lerp(_mouseMovementDelta.y, 0, _lerpValue)
        );

        _lerpValue += 6 * Time.deltaTime;

    }


    private void TapInteraction() {

        bool shortTapAction = false;

        if(Input.GetMouseButtonDown(0)) {
            _tapInteractionTimer = Time.time;
        }

        if(Input.GetMouseButtonUp(0)) {

            if(Time.time - _tapInteractionTimer < 0.2f) {
                shortTapAction = true;
            }

            _tapInteractionTimer = 0;
        }


        if(shortTapAction && _mouseMovementDelta.magnitude < 10) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 100.0f)) {

                if(hit.transform.gameObject.layer == _conveyorRollerLayer) {
                    RollerConveyor conveyor = hit.transform.gameObject.GetComponent<RollerConveyor>();
                    conveyor.ConveyorClicked();
                }
            }
        }
    }
}
