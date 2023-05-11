using UnityEngine;
using UnityEngine.VFX;

public class Package : TransportedObject {

	private bool _packageDestroyed;
    public new void Init(Vector3 objectSize, LevelManager levelManager) {
        _objectSize = objectSize;
        _levelManager = levelManager;
		_packageDestroyed = false;
        ResetObject();
		SetPackageAsNoPhysicsPackage();
    }
    void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.layer == _packageDamageColliderLayer || collision.gameObject.layer == _packageColliderLayer || collision.gameObject.layer == _physicsPackageDamageColliderLayer || collision.gameObject.layer == _physicsPackageLayer) {
			DestroyPackage();

		} else if (collision.gameObject.layer == _deliveryPointCollider) {
			ObjectDestinationReached();
		}

	}

	private void DestroyPackage() {
        if (_packageDestroyed) return;
        _packageDestroyed = true;

        gameObject.SetActive(false);

        string particleObjectDestroyedPoolId = "ParticleObjectDestroyed";
		GameObject particle = PrefabManager.Instance.SpawnFromPool(
			particleObjectDestroyedPoolId,
			transform.position,
			Quaternion.identity
		);
		ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
		if (particleSystem != null) {
			particleSystem.Play();
		}

		_levelManager.PackageDestroyedEvent();
	}

	private void ObjectDestinationReached() {
		gameObject.SetActive(false);

        string objectDeliveredEffectPoolId = "StylizedSmokePoof";

		GameObject effect = PrefabManager.Instance.SpawnFromPool(
			objectDeliveredEffectPoolId,
			transform.position,
			Quaternion.identity
		);
		VisualEffect visualEffect = effect.GetComponent<VisualEffect>();
		if (visualEffect != null) {
			visualEffect.Play();
		}

		_levelManager.PackageDeliveredEvent();
	}

    public void SetPackageAsPhysicsPackage() {
        gameObject.layer = 11;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<Package>().enabled = false;
    }
    public void SetPackageAsNoPhysicsPackage() {
        gameObject.layer = 6;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Package>().enabled = true;
    }
}
