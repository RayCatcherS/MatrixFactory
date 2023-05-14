using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    [Header("Map Elements")]
    [SerializeField] List<Prefab> _mapElements;
	[Header("Particles-Effects")]
    [SerializeField] List<Prefab> _particleEffects;

    
    private Dictionary<string, Queue<GameObject>> _poolPrefab = new Dictionary<string, Queue<GameObject>>();
    private bool _prefabManagerInit = false;

    public static PrefabManager Instance { get; private set; }
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void InitPrefabPool() {
        if(_prefabManagerInit) {
            throw new System.Exception("PrefabManager already initialized");
        }

        GameObject pool = new GameObject();
        pool.name = "ObjectPool";



        for(int i = 0; i < _mapElements.Count; i++) {
            if(_mapElements[i].PoolPrefab) {

                string prefabId = _mapElements[i].PrefabId;
                _poolPrefab.Add(prefabId, new Queue<GameObject>()); // pool creation

                for(int j = 0; j < _mapElements[i].ObjPoolSize; j++) {
                    GameObject obj = Instantiate(_mapElements[i].GetGameobject, pool.transform);
                    obj.SetActive(false);
                    _poolPrefab[prefabId].Enqueue(obj);
                    pool.transform.parent = pool.transform;
                }
            }
        }

        for(int i = 0; i < _particleEffects.Count; i++) {
            if(_particleEffects[i].PoolPrefab) {

                string prefabId = _particleEffects[i].PrefabId;
                _poolPrefab.Add(prefabId, new Queue<GameObject>()); // pool creation

                for(int j = 0; j < _particleEffects[i].ObjPoolSize; j++) {
                    GameObject obj = Instantiate(_particleEffects[i].GetGameobject, pool.transform);
                    obj.SetActive(false);
                    _poolPrefab[prefabId].Enqueue(obj);
                    pool.transform.parent = pool.transform;
                }
            }
        }

        _prefabManagerInit = true;
    }
    public GameObject SpawnFromPool(string poolId, Vector3 prefabPosition, Quaternion prefabQuaternion) { 
        GameObject gameObject = null;

        if (_prefabManagerInit) {
            if (_poolPrefab.ContainsKey(poolId)) {

                gameObject = _poolPrefab[poolId].Dequeue();

                _poolPrefab[poolId].Enqueue(gameObject);
                gameObject.transform.position = prefabPosition;
                gameObject.transform.rotation = prefabQuaternion;
                gameObject.SetActive(true);
            } else {
                throw new System.Exception("Pool not found");
            }
        } else {
            throw new System.Exception("PrefabManager not has not initialized");
        }
        return gameObject;
    }

    public Prefab GetPrefab(string id) {

        for(int i = 0; i < _mapElements.Count; i++) {

            if (_mapElements[i].PrefabId == id) {
                return _mapElements[i];
			}
        }
        for(int i = 0; i < _particleEffects.Count; i++) {
            if(_particleEffects[i].PrefabId == id) {
                return _particleEffects[i];
            }
        }
        return null;
    }


    public void DespawnFromPool(string poolId, GameObject gameObject) {
        if(_poolPrefab.ContainsKey(poolId)) {
            gameObject.SetActive(false);
        } else {
            throw new System.Exception("Pool not found");
        }
    }
}

[System.Serializable]
public class Prefab {
    public Prefab(GameObject gObject, Vector3 gSize) {
        _gObject = gObject;
        _gSize = gSize;
    }
    [Header("Prefab attributes")]
    [SerializeField] private string _prefabID;
	[SerializeField] private GameObject _gObject;
	[SerializeField] private Vector3 _gSize;

    [Header("Prefab pool settings")]
    [SerializeField] private bool _poolPrefab = false;
    [SerializeField] private int _objPoolSize = 10;

    public GameObject GetGameobject {
        get { return _gObject; }
    }
    public Vector3 GameobjectSize {
        get { return _gSize; }
    }
    public string PrefabId {
        get { return _prefabID; }
    }
    public bool PoolPrefab {
        get { return _poolPrefab; }
    }
    public int ObjPoolSize {
        get { return _objPoolSize; }
    }
}
