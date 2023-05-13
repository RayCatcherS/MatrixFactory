using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour {

    public enum Action { Increment, Decrement}

    [Header("Package Collector Settings")]
    [SerializeField] private float _packageSize;
    [SerializeField] private float _packageCollectorOffsetDistance;

    [SerializeField] private int _packageCollectorX;
    [SerializeField] private int _packageCollectorY;
    [SerializeField] private int _packageCollectorZ;

    [SerializeField] private Transform _packageCollectorTarget;
    Vector3Int _packageDeliveryPos = new Vector3Int(0, 0, 0);


    List<GameObject> _packageCollector = new List<GameObject>();

    private int _packageCollectorXSize {
        get { return _packageCollectorX - 1; } 
    }
    private int _packageCollectorYSize {
        get { return _packageCollectorY - 1; }
    }
    private int _packageCollectorZSize {
        get { return _packageCollectorZ - 1; }
    }

    public void InitDeliveryPoint() {
        _packageDeliveryPos = new Vector3Int(0, 0, 0);
        _packageCollector = new List<GameObject>();
    }

    public void VisualPackageAction(Action action) {


        _packageDeliveryPos = IncrementVector(action, _packageDeliveryPos);


        DrawPackages();

    }

    private Vector3Int IncrementVector(Action action, Vector3Int vector) {
        if(action == Action.Increment) {
            if(vector.x < _packageCollectorXSize) {

                vector.x++;
            } else if(vector.z < _packageCollectorZSize) {
                vector.x = 0;
                vector.z++;
            } else if(vector.y < _packageCollectorYSize) {
                vector.x = 0;
                vector.z = 0;
                vector.y++;
            } else {
                Debug.LogError("Package Collector is full");
            }
        } else if(action == Action.Decrement) {

            if(vector.x > 0) {
                vector.x--;
            } else if(vector.z > 0) {
                vector.x = _packageCollectorXSize;
                vector.z--;
            } else if(vector.y > 0) {
                vector.x = _packageCollectorXSize;
                vector.z = _packageCollectorZSize;
                vector.y--;
            } else {
                Debug.LogError("Package Collector is empty");
            }

        }

        return vector;
    }

    private void DrawPackages() {
        Vector3Int visualDeliveryPos = new Vector3Int();
        while(visualDeliveryPos != _packageDeliveryPos) {
            float packageSize = PrefabManager.Instance.GetPrefab("DummyPackage").GameobjectSize.x;

            Vector3 gameObjPos = new Vector3(
                        (visualDeliveryPos.x * (_packageCollectorOffsetDistance + packageSize)) + _packageCollectorTarget.position.x,
                        (visualDeliveryPos.y * (_packageCollectorOffsetDistance + packageSize)) + _packageCollectorTarget.position.y,
                        (visualDeliveryPos.z * (_packageCollectorOffsetDistance + packageSize)) + _packageCollectorTarget.position.z);
            GameObject package = PrefabManager.Instance.SpawnFromPool("DummyPackage", gameObjPos, Quaternion.identity);
            _packageCollector.Add(package);
            visualDeliveryPos = IncrementVector(Action.Increment, visualDeliveryPos);

        }
    }

    public void WipeDeliveryPackages() {
        for(int i = 0; i < _packageCollector.Count; i++) {
            _packageCollector[i].SetActive(false);
            Debug.Log("Package Collector " + i + " set to false");
        }
    }
}
