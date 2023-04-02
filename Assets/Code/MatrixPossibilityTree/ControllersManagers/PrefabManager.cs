using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    [SerializeField] private GameObject _conveyorBelt;
    public Prefab conveyorBelt {
        get { return new Prefab(_conveyorBelt, new Vector3(1, 1, 1)); }
    }
}

public class Prefab {
    public Prefab(GameObject gObject, Vector3 gSize) {
        _gObject = gObject;
        _gSize = gSize;
    }
    private GameObject _gObject;
    private Vector3 _gSize;

    public GameObject gObject {
        get { return _gObject; }
    }
    public Vector3 gSize {
        get { return _gSize; }
    }
}
