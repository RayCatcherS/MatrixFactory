using UnityEngine;
using UnityEngine.VFX;

public class Package : TransportedObject {

	void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.layer == _packageDamageColliderLayer || collision.gameObject.layer == _packageColliderLayer) {
			DestroyPackage();

		}
		else if (collision.gameObject.layer == _deliveryPointCollider) {
			ObjectDestinationReached();
		}

	}

	private void DestroyPackage() {
		gameObject.SetActive(false);

		GameObject particle = Instantiate(_objectDestroyedParticles, transform.position, Quaternion.identity);
		ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
		if (particleSystem != null) {
			particleSystem.Play();
		}

		_levelManager.PackageDestroyedEvent();
	}

	private void ObjectDestinationReached() {
		gameObject.SetActive(false);

		GameObject particle = Instantiate(_objectDeliveredEffect, transform.position, Quaternion.identity);
		VisualEffect visualEffect = particle.GetComponent<VisualEffect>();
		if (visualEffect != null) {
			visualEffect.Play();
		}

		_levelManager.PackageDeliveredEvent();
	}
}
