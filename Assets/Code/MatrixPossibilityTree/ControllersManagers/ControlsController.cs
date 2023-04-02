using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsController : MonoBehaviour {

    [SerializeField] private CameraController cameraController;

    [SerializeField] private float rotationSpeed = 100f;
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

        Vector2 rotate = rotationSpeed * Time.deltaTime * mouseMovementDelta;


        //cameraController.transform.eulerAngles += new Vector3(/*rotate.x*/0, rotate.x, 0);
    }
}
