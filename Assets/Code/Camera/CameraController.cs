using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    void Start() {

    }

    public void SetRotation(Vector3 pos) {
        CinemachinePOV _pov;
        _pov = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _pov.m_HorizontalAxis.Value = pos.x;
        _pov.m_VerticalAxis.Value = pos.y;
    }
    public void ResetRotation() {
        SetRotation(new Vector3(-135, 35, 0));
    }
    public void SetCameraTarget(Vector3 pos) {
        cameraTarget.transform.position = pos;
    }
}
