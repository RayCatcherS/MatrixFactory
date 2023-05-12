using UnityEngine;
using UnityEngine.VFX;
using static Package;

public class Package : TransportedObject {
    public enum PackageType {
        normal,
        bomb
    }
    private bool _packageDestroyed;
	private PackageType _packageType;

	[Header("Package References")]
	[SerializeField] private GameSignal _bombSignal;

    public new void Init(Vector3 objectSize, LevelManager levelManager, PackageType packageType) {
        _objectSize = objectSize;
        _levelManager = levelManager;


        _packageDestroyed = false;

        SetPackageType(packageType);
        ResetObject();
		SetPackageAsNoPhysicsPackage();
    }

	

    void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.layer == _packageDamageColliderLayer || collision.gameObject.layer == _packageColliderLayer || collision.gameObject.layer == _physicsPackageDamageColliderLayer || collision.gameObject.layer == _physicsPackageLayer) {
			DestroyPackage();

		} else if (collision.gameObject.layer == _deliveryPointCollider) {

			if(_packageType == PackageType.bomb) {
                DestroyPackage();
            } else if(_packageType == PackageType.normal) {
                ObjectDestinationReached();
            }
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

    public void SetPackageAsPhysicsPackage(Vector3 direction = new Vector3()) {
        gameObject.layer = 11;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;


        gameObject.GetComponent<Rigidbody>().velocity = Vector3.down * _objectVelocity;
        this.gameObject.GetComponent<Package>().enabled = false;
    }
    public void SetPackageAsNoPhysicsPackage() {
        gameObject.layer = 6;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Package>().enabled = true;
    }

	private void SetPackageType(PackageType type) {
        _packageType = type;

		if(_packageType == PackageType.bomb) {
			_bombSignal.StartSignal();
		} else {
			_bombSignal.StopSignal();

        }
    }
}
