using PT.DataStruct;
using PT.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static PT.Global.GlobalPossibilityPath;

public class LevelManager : MonoBehaviour {

    [Header("Game Components")]

    [SerializeField] private GameController _gameController;
    [SerializeField] private CameraController _cameraController;
    private PrefabManager _prefabManager;
    
    


    [Header("Conveyor Belt Generation")]
    private readonly float _CBGenerationOffsetHeight = 0.25f;
    private Vector3 _mapPositionStart = Vector3.zero;
    private Vector3 _mapCenter = Vector3.zero;
    private ConveyorBelt[,] _conveyorMap;
    private List<ConveyorBelt> _conveyorMapList = new List<ConveyorBelt>();
    private float _conveyorMaxHeight = 0;

    [Header("Delivery Point Generation")]
    private GameObject _deliveryPoint;

    [Header("Package Generation")]
    private List<Package> _packages = new List<Package>();
    private int _packagesToSpawn = 0;
    private int _packagesDestroyed = 0;
    private int _packagesDelivered = 0;


    [Header("Level")]
    [SerializeField] private Transform _packageSpawnTransform;
    private float _packageSpawnOffsetHeight = 5;
    [SerializeField] private GameObject _spawnLight;
    private float _lightHeightOffset = 2;
    private GeneratedLevel _loadedLevel;


    public Vector3 MapCenter {
        get { return _mapCenter; }
    }

    private LevelInfo _levelInfo;
    public LevelInfo LevelInfo {

        get {
            if(_levelInfo.Chapter == Chapter.NotSelected || _levelInfo.LevelIndex == -1) {
                throw new System.InvalidOperationException("No Chapter or level selected");
            }
            return _levelInfo;
        }
    }


    [SerializeField] private bool _debugPath = false;
    private bool _levelInitialized = false;

	public void InitLevel(Chapter chapter, int levelIndex) {
        _levelInfo = new LevelInfo(chapter, levelIndex);


        InitGameComponents();

        _loadedLevel = GlobalPossibilityPath.GetChapterLevels(chapter)[levelIndex];

        InitMatrixMap(_loadedLevel);

        _cameraController.SetRotation(new Vector3(-135, 35, 0)); // reset camera rotation
        _cameraController.SetCameraTarget(_mapCenter);
        SetPackageSpawnPosition(_loadedLevel);


        _packagesToSpawn = _loadedLevel.packageToSpawn;
        _packagesDestroyed = 0;

        _levelInitialized = true;
	}

    public void DestroyLevel() {

        if(_levelInitialized) {
            _levelInitialized = false;
            _loadedLevel = null;
            _levelInfo = new LevelInfo(Chapter.NotSelected, -1);
            _packagesDelivered = 0;
            _packagesToSpawn = 0;

            Destroy(_deliveryPoint);

            foreach(ConveyorBelt conveyor in _conveyorMapList) {
                Destroy(conveyor.gameObject);
            }
            _conveyorMapList.Clear();

            foreach(Package package in _packages) {
                Destroy(package.gameObject);
            }
            _packages.Clear();
        }
    }

    private void InitGameComponents() {
        _prefabManager = gameObject.GetComponent<PrefabManager>();
    }

    private void InitMatrixMap(GeneratedLevel levelPath) {

        if (_levelInitialized) {

        }

        int rows = levelPath.MatrixSize().x;
        int columns = levelPath.MatrixSize().y;
        _conveyorMap = new ConveyorBelt[rows, columns];
        


        
        Vector3 conveyorBlockOffset = Vector3.zero;



        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int pathLength = levelPath.PathElements.Count;
        int heightRangeMin = 0;
        int heightRangeMax = pathLength;


        /* INIT GAMEOBJECT CONVEYOR MAP*/
        // create random conveyor height
        for(int r = 0; r < rows; r++) {

            conveyorBlockOffset = new Vector3(conveyorBlockOffset.x, 0, 0);
            for(int c = 0; c < columns; c++) {
                

                GameObject obj = Instantiate(_prefabManager.ConveyorBelt.GetGameobject, _mapPositionStart + conveyorBlockOffset, Quaternion.identity);

                ConveyorBelt conveyorBelt = obj.GetComponent<ConveyorBelt>();
                _conveyorMapList.Add(conveyorBelt);
                _conveyorMap[r, c] = conveyorBelt;


                /* SET CONVEYOR RANDOM ROTATION */
                SecureRandom randomRotationSteps = new SecureRandom();
                int translations = randomRotationSteps.Next(0, 3);
                Direction rollerConveyorDirection = Direction.stay;

                if(translations == 0) {
                    rollerConveyorDirection = Direction.forward;
                } else if (translations == 1) {
                    rollerConveyorDirection = Direction.right;
                } else if (translations == 2) {
                    rollerConveyorDirection = Direction.back;
                } else if (translations == 3) {
                    rollerConveyorDirection = Direction.left;
                }



                /* SET CONVEYOR RANDOM HEIGHT(in range) */
                System.Random random = new System.Random();
                int conveyorUnitHeight = random.Next(heightRangeMin, heightRangeMax);


                /* INIT NEW CONVEYOR*/
                random = new System.Random();
                bool conveyorIsDestroyer = random.Next(0, 4) == 3 ? true : false;
                if(conveyorIsDestroyer) {
                    conveyorBelt.InitConveyorBelt(conveyorUnitHeight * _CBGenerationOffsetHeight, rollerConveyorDirection, ConveyorBelt.ConveyorBeltType.destroyer);
                } else {
                    conveyorBelt.InitConveyorBelt(conveyorUnitHeight * _CBGenerationOffsetHeight, rollerConveyorDirection, ConveyorBelt.ConveyorBeltType.roller);
                }


                // Set/Update the highest conveyor on the map
                if(_conveyorMaxHeight < conveyorBelt.RollerConveyorHeight) {
                    _conveyorMaxHeight = conveyorBelt.RollerConveyorHeight;
                }


                // increment column offset
                conveyorBlockOffset = conveyorBlockOffset + new Vector3(0, 0, _prefabManager.ConveyorBelt.GameobjectSize.x);
            }

            // increment row offset
            conveyorBlockOffset = conveyorBlockOffset + new Vector3(_prefabManager.ConveyorBelt.GameobjectSize.x, 0, 0);
        }



        /* INIT GOOD END PATH CONVEYOR CORRECT HEIGHT*/
        for(int i = 0; i < levelPath.PathElements.Count; i++) {

            double conveyorHeight;
            
            if(i != 0) {
                System.Random random = new System.Random();
                int randomNextHeight = random.Next(i - 1, i + 1); // the random height has value between the current offset and the previous one

                conveyorHeight = heightRangeMin + (levelPath.PathElements.Count - randomNextHeight) * _CBGenerationOffsetHeight;
            } else {
                conveyorHeight = heightRangeMin + (levelPath.PathElements.Count - i) * _CBGenerationOffsetHeight;
            }


            ConveyorBelt pathCurrentConveyor = _conveyorMap[levelPath.PathElements[i].pos.x, levelPath.PathElements[i].pos.y];
            /* INIT NEW CONVEYOR*/
            pathCurrentConveyor.SetConveyorType(ConveyorBelt.ConveyorBeltType.roller);
            pathCurrentConveyor.SetRollerConveyorHeight(conveyorHeight);

            // Set/Update the highest conveyor on the map
            if(_conveyorMaxHeight < pathCurrentConveyor.RollerConveyorHeight) {
                _conveyorMaxHeight = pathCurrentConveyor.RollerConveyorHeight;
            }

            // show path
            if(_debugPath) {
                pathCurrentConveyor.EnableDebugShowPath(true);
            }
        }


        /* SPAWN DELIVERY POINT*/
        Vector3 deliveryPointPos = new Vector3(
            (levelPath.EndPathPosition().x * _prefabManager.ConveyorBelt.GameobjectSize.x),
            0,
            (levelPath.EndPathPosition().y * _prefabManager.ConveyorBelt.GameobjectSize.x) + (_prefabManager.ConveyorBelt.GameobjectSize.x)
        );
        _deliveryPoint = Instantiate(_prefabManager.DeliveryPoint.GetGameobject, deliveryPointPos, Quaternion.identity);


        /* CALCULATE CENTER OF THE MAP */
        // this can be used to center the game camera

        float _mapCenterHeight = _conveyorMaxHeight / 2;
        _mapCenter = new Vector3(
            (levelPath.MatrixSize().x * _prefabManager.ConveyorBelt.GameobjectSize.y) / 2,
            _mapCenterHeight,
            (levelPath.MatrixSize().y * _prefabManager.ConveyorBelt.GameobjectSize.x) / 2
        );
    }



    private void SetPackageSpawnPosition(GeneratedLevel levelPat) {
        _packageSpawnTransform.transform.position = new Vector3(
            levelPat.StartPathPosition().x + (_prefabManager.ConveyorBelt.GameobjectSize.x / 2),
            _conveyorMaxHeight + _packageSpawnOffsetHeight,
            levelPat.StartPathPosition().y + (_prefabManager.ConveyorBelt.GameobjectSize.y / 2)
        );
    }


    public void StartLevel() {
        if(!_levelInitialized) {
            throw new System.InvalidOperationException("There is no initilized level");
		}

        _spawnLight.gameObject.SetActive(false);

        _spawnLight.gameObject.transform.position = new Vector3(
            _packageSpawnTransform.position.x,
            _conveyorMaxHeight + _lightHeightOffset,
            _packageSpawnTransform.position.z
        );

        

        StartCoroutine(WaitAndStartLight());
    }

    private IEnumerator WaitAndStartLight() {

        yield return new WaitForSeconds(2);
        _spawnLight.gameObject.SetActive(true);


        StartCoroutine(WaitAndSpawnPackage());

        yield return new WaitForSeconds(2.5f);
        _spawnLight.gameObject.SetActive(false);
    }

    private IEnumerator WaitAndSpawnPackage() {

        if(_packagesToSpawn > 1) {
            yield return new WaitForSeconds(2);
            GameObject obj = Instantiate(_prefabManager.Package.GetGameobject, _packageSpawnTransform.position, Quaternion.identity);
            Package package = obj.GetComponent<Package>();
            _packages.Add(package);
            package.Init(
                _prefabManager.Package.GameobjectSize,
                _prefabManager.PackageDestroyedParticles.GetGameobject,
                _prefabManager.PackageDeliveredEffect.GetGameobject,
                this
            );

            yield return new WaitForSeconds(0.7f);
            StartCoroutine(WaitAndSpawnPackage());

            _packagesToSpawn--;
        }
    }

    public void PackageDestroyedEvent() {
        _packagesDestroyed++;
        EvaluateEndLevelStatus();
    }

    public void PackageDeliveredEvent() {
        _packagesDelivered++;
        EvaluateEndLevelStatus();
    }

    private void EvaluateEndLevelStatus() {

        if(IsLevelEnded()) {
            if(IsLevelLose()) {
                _gameController.EndLevelLose();
            } else {
                _gameController.EndLevelWin();
            }
        }
    }

    private bool IsLevelEnded() {
        return _loadedLevel.packageToSpawn <= _packagesDestroyed + _packagesDelivered;
    }

    private bool IsLevelLose() {
        return _packagesDestroyed >= _loadedLevel.packageToSpawn / 2;
    }
}
