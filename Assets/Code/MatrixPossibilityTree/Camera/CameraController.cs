using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Transform cameraTarget;
    void Start() {

    }

    public void SetCameraTarget(Vector3 pos) {
        cameraTarget.transform.position = pos;
    }
}
