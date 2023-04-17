using UnityEngine;

public class Trail : TransportedObject {

    private TrailRenderer _trailRenderer;
    public new void Init(Vector3 objectSize, LevelManager levelManager) {
        

        base.Init(objectSize, levelManager);
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if(_trailRenderer != null) {
            _trailRenderer.Clear();
        }
    }


    void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.layer == _packageDamageColliderLayer || collision.gameObject.layer == _packageColliderLayer || collision.gameObject.layer == _deliveryPointCollider) {

			// redraw trail, call a method from level manager to redraw a trail
            _levelManager.DrawTrailIndicator();
		}

	}

	
}
