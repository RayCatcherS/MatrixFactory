using UnityEngine;
using UnityEngine.VFX;
using static Package;

public class Package : TransportedObject {
    public enum PackageType {
        normal,
        bomb
    }
    private bool _packageDestroyed;
    [SerializeField] private PackageType _packageType;
	[SerializeField] private int _bombDamage;

	[Header("Package References")]
	[SerializeField] private GameSignal _bombSignal;
    [SerializeField] private GameObject _yellowTape;

    [Header("Package Sounds References")]
    [SerializeField] private AudioClip _packageDestroyedSound;
    [SerializeField] private AudioClip _packageBombDestroyedSound;

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
				_levelManager.DeliveryPointDamage(_bombDamage);
            } else if(_packageType == PackageType.normal) {
                ObjectDestinationReached();
            }
		}

	}

	private void DestroyPackage() {
        if (_packageDestroyed) return;
        _packageDestroyed = true;

        PrefabManager.Instance.DespawnFromPool("Package", gameObject);


        if(_packageType == PackageType.bomb) {
            GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
            audioSource.GetComponent<AudioSource>().clip = _packageBombDestroyedSound;
            audioSource.GetComponent<AudioSource>().Play();

            gameObject.GetComponent<ObjectDestoyEffect>().substituteOfDestroyedObjectPoolId = "";
            gameObject.GetComponent<ObjectDestoyEffect>().particleEffectPoolId = "ParticleObjectDestroyed";
        } else if (_packageType == PackageType.normal) {
            GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
            audioSource.GetComponent<AudioSource>().clip = _packageDestroyedSound;
            audioSource.GetComponent<AudioSource>().Play();


            gameObject.GetComponent<ObjectDestoyEffect>().particleEffectPoolId = "";
            gameObject.GetComponent<ObjectDestoyEffect>().substituteOfDestroyedObjectPoolId = "DamagedPackage";
        }

        gameObject.GetComponent<ObjectDestoyEffect>().StartDestroyEffect();

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
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            particleSystem.Play();
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
            _yellowTape.SetActive(true);

        } else {
			_bombSignal.StopSignal();
            _yellowTape.SetActive(value: false);

        }
    }
}
