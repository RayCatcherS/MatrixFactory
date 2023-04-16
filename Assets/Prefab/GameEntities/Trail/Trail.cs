using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : TransportedObject {

    void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.layer == _packageDamageColliderLayer || collision.gameObject.layer == _packageColliderLayer || collision.gameObject.layer == _deliveryPointCollider) {

			// redraw trail, call a method from level manager to redraw a trail

		}

	}

	
}
