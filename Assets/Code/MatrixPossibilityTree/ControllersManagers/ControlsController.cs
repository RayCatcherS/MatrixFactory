using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsController : MonoBehaviour {

    [SerializeField] private CameraController cameraController;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;

    [SerializeField] private float XrotationSpeed;
    [SerializeField] private float YrotationSpeed;
    private bool dragRotationActive = false;

    private Vector2 lateMousePosition = Vector2.zero;
    Vector2 mouseMovementDelta = Vector2.zero;
    

    void Start() {

    }


    void Update() {

        HandleCameraRotation();
    }

    private void HandleCameraRotation() {

        if(Input.GetMouseButtonDown(0)) {

            dragRotationActive = true;
            lateMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0)) {

            dragRotationActive = false;
        }

        if(dragRotationActive) {
            mouseMovementDelta = (Vector2)Input.mousePosition - lateMousePosition;


            lateMousePosition = Input.mousePosition;
        }

        float rotationXValue = XrotationSpeed * Time.deltaTime * mouseMovementDelta.x;
        float rotationYValue = YrotationSpeed * Time.deltaTime * mouseMovementDelta.y;

        // set rotation to inuput axis chinemachine value
        cinemachineFreeLook.m_XAxis.m_InputAxisValue = rotationXValue;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = rotationYValue;
    }
}
