using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeliveryPoint : MonoBehaviour {

    public enum Action { Increment, Decrement}

    [Header("Package Collector Settings")]
    [SerializeField] private float _packageSize;
    [SerializeField] private float _packageCollectorPadding;
    [SerializeField] private int _packageCollectorX;
    [SerializeField] private int _packageCollectorY;
    [SerializeField] private int _packageCollectorZ;

    

    [Header("Package Collector References")]
    [SerializeField] private Transform _packageCollectorTarget;
    [SerializeField] private BoxCollider _boxCollider;
    List<GameObject> _packageCollector = new List<GameObject>();

    [Header("Delivery Point References")]
    [SerializeField] private GameObject _deliveryPointIcon;

    private bool _isDeliveryPointActive = true;


    Vector3Int _packageDeliveryPos = new Vector3Int(0, 0, 0);

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
        CalculateBoxColliderSize();

        SetActiveDeliveryIconAnimation(true);
    }

    public void VisualPackageAction(Action action) {


        _packageDeliveryPos = IncrementVectorPosition(action, _packageDeliveryPos);


        if(action == Action.Decrement) {
            _packageCollector.Last().gameObject.GetComponent<ObjectDestoyEffect>().StartDestroyEffect();
        }

        DrawDummyPackages();
        CalculateBoxColliderSize();
    }

    private Vector3Int IncrementVectorPosition(Action action, Vector3Int vector) {
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
            AnimateDeliveryIcon();


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
            AnimateDeliveryIcon();

        }

        return vector;
    }

    private void DrawDummyPackages() {
        Vector3Int visualDeliveryPos = new Vector3Int();

        WipeDeliveryPackages();

        while(visualDeliveryPos != _packageDeliveryPos) {
            float packageSize = PrefabManager.Instance.GetPrefab("DummyPackage").GameobjectSize.x;

            Vector3 gameObjPos = new Vector3(
                        (visualDeliveryPos.x * (_packageCollectorPadding + packageSize)) + _packageCollectorTarget.position.x + packageSize / 2,
                        (visualDeliveryPos.y * (_packageCollectorPadding + packageSize)) + _packageCollectorTarget.position.y + packageSize / 2,
                        (visualDeliveryPos.z * (_packageCollectorPadding + packageSize)) + _packageCollectorTarget.position.z + packageSize / 2);
            GameObject package = PrefabManager.Instance.SpawnFromPool("DummyPackage", gameObjPos, Quaternion.identity);
            _packageCollector.Add(package);
            visualDeliveryPos = IncrementVectorPosition(Action.Increment, visualDeliveryPos);

        }
    }

    private void CalculateBoxColliderSize() {
        float xSize = _packageCollectorX * (_packageSize + _packageCollectorPadding);
        float ySize = _packageDeliveryPos.y * (_packageSize + _packageCollectorPadding);
        float zSize = _packageCollectorZ * (_packageSize + _packageCollectorPadding);

        if(ySize == 0) {
            ySize = 0.1f;
        }

        _boxCollider.size = new Vector3(xSize, ySize, zSize);
        _boxCollider.center = new Vector3(xSize / 2, ySize / 2, zSize / 2);
        _boxCollider.gameObject.transform.position = _packageCollectorTarget.position;
    }

    public void WipeDeliveryPackages() {
        for(int i = 0; i < _packageCollector.Count; i++) {
            PrefabManager.Instance.DespawnFromPool(
                "DummyPackage",
                _packageCollector[i]
            );
        }
    }

    public void SetActiveDeliveryIconAnimation(bool active) {

        if(_isDeliveryPointActive != active) {
            if(active) {

                gameObject.GetComponent<Animator>().SetTrigger("enable");
                Debug.Log("Delivery Point Icon Animation Enabled");
            } else {
                gameObject.GetComponent<Animator>().SetTrigger("disable");

                gameObject.GetComponent<Animator>().SetTrigger("disable");
            }
        }

        _isDeliveryPointActive = active;
    }


    private void AnimateDeliveryIcon() { 
        gameObject.GetComponent<Animator>().SetTrigger("IconAction");
    }
}
