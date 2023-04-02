
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    public CameraController cameraController {
        get { return _cameraController; }
    }

    void Start() {
        
    }
}
