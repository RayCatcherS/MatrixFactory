using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    [Header("Map Elements")]
    [SerializeField] List<Prefab> _mapElements;
    [SerializeField] private GameObject _trailPackage;
	[Header("Particles-Effects")]
    [SerializeField] private GameObject _packageDestroyedParticles;
    [SerializeField] private GameObject _packageDeliveredEffect;




	public Prefab TrailPackage
	{
		get { return new Prefab(_trailPackage, new Vector3(0.3f, 0.3f, 0.3f)); }
	}
	public Prefab PackageDestroyedParticles {
        get { return new Prefab(_packageDestroyedParticles, new Vector3(1, 1, 1)); }
    }
    public Prefab PackageDeliveredEffect {
        get { return new Prefab(_packageDeliveredEffect, new Vector3(1, 1, 1)); }
    }

    public Prefab GetPrefab(string id) {

        for(int i = 0; i < _mapElements.Count; i++) {

            if (_mapElements[i].PrefabId == id) {
                return _mapElements[i];
			}
        }
        return null;
    }
}

[System.Serializable]
public class Prefab {
    public Prefab(GameObject gObject, Vector3 gSize) {
        _gObject = gObject;
        _gSize = gSize;
    }
    [SerializeField] private string _prefabID;
	[SerializeField] private GameObject _gObject;
	[SerializeField] private Vector3 _gSize;

    public GameObject GetGameobject {
        get { return _gObject; }
    }
    public Vector3 GameobjectSize {
        get { return _gSize; }
    }
    public string PrefabId {
        get { return _prefabID; }
    }
}
