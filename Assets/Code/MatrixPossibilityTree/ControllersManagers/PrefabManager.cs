using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    [SerializeField] private GameObject _conveyorBelt;
    [SerializeField] private GameObject _package;

    public Prefab conveyorBelt {
        get { return new Prefab(_conveyorBelt, new Vector3(1, 1, 1)); }
    }

    public Prefab package {
        get { return new Prefab(_package, new Vector3(0.3f, 0.3f, 0.3f)); }
    }
}

public class Prefab {
    public Prefab(GameObject gObject, Vector3 gSize) {
        _gObject = gObject;
        _gSize = gSize;
    }
    private GameObject _gObject;
    private Vector3 _gSize;

    public GameObject GetGameobject {
        get { return _gObject; }
    }
    public Vector3 GameobjectSize {
        get { return _gSize; }
    }
}
