using UnityEngine;

public class TrailIndicator : TransportedObject {

    private TrailRenderer _trailRenderer;
    public new void Init(Vector3 objectSize, LevelManager levelManager) {
        

        base.Init(objectSize, levelManager);
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if(_trailRenderer != null) {
            _trailRenderer.Clear();
        }

        _objectFallSpeed = 2f;
    }

}
