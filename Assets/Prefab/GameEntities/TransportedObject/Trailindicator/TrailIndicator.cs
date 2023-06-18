using UnityEngine;
using UnityEngine.Pool;

public class TrailIndicator : TransportedObject {

    private bool _packageDestroyed;
    private TrailRenderer _trailRenderer;

    private readonly string trailGOId = "LevelMapTrail";
    public new void Init(Vector3 objectSize, LevelManager levelManager) {

        base.Init(objectSize, levelManager);
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
        _packageDestroyed = false;
        if(_trailRenderer != null) {
            _trailRenderer.Clear();
        }

        _objectFallSpeed = 2f;
		this.enabled = true;
    }



	void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.layer == _packageDamageColliderLayer ||
            collision.gameObject.layer == _packageColliderLayer ||
            collision.gameObject.layer == _physicsPackageDamageColliderLayer ||
            collision.gameObject.layer == _physicsPackageLayer) {


            gameObject.GetComponent<ObjectDestoyEffect>().particleEffectPoolId = "TrailCollisionIndicator";
            DestroyTrail();
            

		} else if (collision.gameObject.layer == _deliveryPointCollider) {

            gameObject.GetComponent<ObjectDestoyEffect>().particleEffectPoolId = "TrailDeliveryCollisionIndicator";
            DestroyTrail();
        }


    }

    private void DestroyTrail() {
        if(_packageDestroyed) return;
        _packageDestroyed = true;

        gameObject.GetComponent<ObjectDestoyEffect>().StartDestroyEffect();
        this.enabled = false;
    }
}
