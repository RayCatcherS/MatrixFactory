using PT.DataStruct;
using PT.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PT.Global.GlobalPossibilityPath;

public class LevelManager : MonoBehaviour {
    public enum LevelState {
        NotStarted,
        Started,
        Paused,
        FinishedLose,
        FinishedWin
    }

    [Header("Game Components")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private CameraController _cameraController;
    


    [Header("Conveyor Belt Generation")]
    private readonly float _conveyorPlatformOffsetHeight = 0.25f;
    private Vector3 _mapPositionStart = Vector3.zero;
    private Vector3 _mapCenter = Vector3.zero;
    private ConveyorBelt[,] _conveyorMap;
    private List<ConveyorBelt> _conveyorMapList = new List<ConveyorBelt>();
    private float _conveyorMaxHeight = 0;

    [Header("Delivery Point Generation")]
    private GameObject _deliveryPoint;

    [Header("Package Generation")]
    private int _packagesToSpawn = 0;
    private int _packagesDestroyed = 0;
    private int _packagesDelivered = 0;


    [Header("Level")]
    [SerializeField] private Transform _packageSpawnTransform;
    private float _packageSpawnOffsetHeight = 5;
    [SerializeField] private GameObject _spawnLight;
    private float _lightHeightOffset = 2;
    private GeneratedLevel _loadedLevel;
    private LevelState _levelState = LevelState.NotStarted;

    public LevelState State {
        get { return _levelState; }
    }


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
    private bool _levelLoaded = false;

    /// <summary>
    /// Init level, generate the map and set the level variables
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="levelIndex"></param>
	public void LoadLevel(LevelInfo levelInfo) {
        if(_levelLoaded) {
            throw new System.InvalidOperationException("Another level is already loaded, unload the the previous one");
        }

        _levelInfo = levelInfo;
        _loadedLevel = GlobalPossibilityPath.GetChapterLevels(levelInfo.Chapter)[levelInfo.LevelIndex];
        

        GenerateLevelMap(_loadedLevel);
        SetPackageSpawnPosition(_loadedLevel);


        _packagesToSpawn = _loadedLevel.packageToSpawn;


        _cameraController.ResetRotation();
        _cameraController.SetCameraTarget(_mapCenter);

        _levelLoaded = true;

    }

    /// <summary>
    /// Destroy all the level gameobjects and reset the level variables
    /// </summary>
    public void WipeLevel() {

        if(_levelLoaded) {
            _levelLoaded = false;
            _loadedLevel = null;
            _levelInfo = new LevelInfo(Chapter.NotSelected, -1);
            _packagesDelivered = 0;
            _packagesToSpawn = 0;
            _packagesDestroyed = 0;

            Destroy(_deliveryPoint);

            foreach(ConveyorBelt conveyor in _conveyorMapList) {
                Destroy(conveyor.gameObject);
            }
            _conveyorMapList.Clear();

            //PrefabManager.Instance.ReinitPool();

            _levelState = LevelState.NotStarted;
        }
    }

    private void GenerateLevelMap(GeneratedLevel levelPath) {

        InitMap(levelPath);
        GenerateMapConveyors(levelPath);
        GeneratePath(levelPath);
        GenerateDeliveryPoint(levelPath);
        CalculateCenterOfTheMap(levelPath);
    }
    /// <summary>
    /// Init the conveyor map
    /// </summary>
    /// <param name="levelPath"></param>
    private void InitMap(GeneratedLevel levelPath) {
        int rows = levelPath.MatrixSize().x;
        int columns = levelPath.MatrixSize().y;
        _conveyorMap = new ConveyorBelt[rows, columns];
    }
    /// <summary>
    /// Generate the gameobjects (conveyors) on the map and set the conveyor map
    /// </summary>
    /// <param name="levelPath"></param>
    private void GenerateMapConveyors(GeneratedLevel levelPath) {

        int rows = levelPath.MatrixSize().x;
        int columns = levelPath.MatrixSize().y;
        Vector3 conveyorBaseBlockOffset = Vector3.zero;
        int pathLength = levelPath.PathElements.Count;

        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int conveyorPlatformHeightRangeMin = 0;
        int conveyorPlatformHeightRangeMax = pathLength;


        // conveyor prefab data
        Prefab conveyorPrefab = PrefabManager.Instance.GetPrefab("ConveyorBelt");

		/* INIT GAMEOBJECT CONVEYOR MAP*/
		// instantiate conveyor with random height
		for (int r = 0; r < rows; r++) {

            conveyorBaseBlockOffset = new Vector3(conveyorBaseBlockOffset.x, 0, 0);
            for(int c = 0; c < columns; c++) {


                GameObject obj = Instantiate(conveyorPrefab.GetGameobject, _mapPositionStart + conveyorBaseBlockOffset, Quaternion.identity);

                ConveyorBelt conveyorBelt = obj.GetComponent<ConveyorBelt>();
                _conveyorMapList.Add(conveyorBelt);
                _conveyorMap[r, c] = conveyorBelt;


                /* SET CONVEYOR PLATFORM RANDOM ROTATION */
                SecureRandom randomPlatformRotationSteps = new SecureRandom();
                int translations = randomPlatformRotationSteps.Next(0, 3);
                Direction conveyorPlatformDirection = Direction.stay;

                if(translations == 0) {
                    conveyorPlatformDirection = Direction.forward;
                } else if(translations == 1) {
                    conveyorPlatformDirection = Direction.right;
                } else if(translations == 2) {
                    conveyorPlatformDirection = Direction.back;
                } else if(translations == 3) {
                    conveyorPlatformDirection = Direction.left;
                }



                /* SET CONVEYOR RANDOM PLATFORM HEIGHT(in range) */
                System.Random random = new System.Random();
                int conveyorPlatformUnitHeight = random.Next(conveyorPlatformHeightRangeMin, conveyorPlatformHeightRangeMax);


                /* INIT NEW CONVEYOR*/
                conveyorBelt.InitConveyorBelt(conveyorPlatformUnitHeight * _conveyorPlatformOffsetHeight, conveyorPlatformDirection, ConveyorBelt.ConveyorBeltPlatformType.Roller);


                // Set/Update the highest conveyor on the map
                if(_conveyorMaxHeight < conveyorBelt.RollerConveyorHeight) {
                    _conveyorMaxHeight = conveyorBelt.RollerConveyorHeight;
                }


                // increment column offset
                conveyorBaseBlockOffset = conveyorBaseBlockOffset + new Vector3(0, 0, conveyorPrefab.GameobjectSize.x);
            }

            // increment row offset
            conveyorBaseBlockOffset = conveyorBaseBlockOffset + new Vector3(conveyorPrefab.GameobjectSize.x, 0, 0);
        }
    }
    /// <summary>
    /// Set map conveyors with correct height for the path
    /// </summary>
    /// <param name="levelPath"></param>
    private void GeneratePath(GeneratedLevel levelPath) {

        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int conveyorHeightRangeMin = 0;


        /* INIT GAME PATH CONVEYOR CORRECT HEIGHT*/
        for(int i = 0; i < levelPath.PathElements.Count; i++) {

            double conveyorPlatformHeight;

            if(i != 0) {
                System.Random random = new System.Random();
                int randomNextHeight = random.Next(i - 1, i + 1); // the random height has value between the current offset and the previous one

                conveyorPlatformHeight = conveyorHeightRangeMin + (levelPath.PathElements.Count - randomNextHeight) * _conveyorPlatformOffsetHeight;
            } else {
                conveyorPlatformHeight = conveyorHeightRangeMin + (levelPath.PathElements.Count - i) * _conveyorPlatformOffsetHeight;
            }


            ConveyorBelt pathCurrentConveyor = _conveyorMap[levelPath.PathElements[i].pos.x, levelPath.PathElements[i].pos.y];
            /* INIT NEW CONVEYOR*/
            pathCurrentConveyor.SetConveyorType(levelPath.PathElements[i].conveyorBeltPlatformType);
            pathCurrentConveyor.SetRollerConveyorHeight(conveyorPlatformHeight);

            // Set/Update the highest conveyor on the map
            if(_conveyorMaxHeight < pathCurrentConveyor.RollerConveyorHeight) {
                _conveyorMaxHeight = pathCurrentConveyor.RollerConveyorHeight;
            }

            // show path
            if(_debugPath) {
                pathCurrentConveyor.EnableDebugShowPath(true);
            }
        }
    }
   
    /// <summary>
    /// Generate the delivery point onto the map
    /// </summary>
    /// <param name="levelPath"></param>
    private void GenerateDeliveryPoint(GeneratedLevel levelPath) {

		// deliveryPoint prefab data
		Prefab deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPoint");


		/* SPAWN DELIVERY POINT*/
		Vector3 deliveryPointPos = new Vector3(
            (levelPath.EndPathPosition().x * deliveryPoint.GameobjectSize.x),
            0,
            (levelPath.EndPathPosition().y * deliveryPoint.GameobjectSize.x) + (deliveryPoint.GameobjectSize.x)
        );
        _deliveryPoint = Instantiate(deliveryPoint.GetGameobject, deliveryPointPos, Quaternion.identity);
    }
    /// <summary>
    /// Claculate the center of the map
    /// </summary>
    /// <param name="levelPath"></param>
    private void CalculateCenterOfTheMap(GeneratedLevel levelPath) {

		// conveyor prefab data
		Prefab conveyorPrefab = PrefabManager.Instance.GetPrefab("ConveyorBelt");

		/* CALCULATE CENTER OF THE MAP */
		// this can be used to center the game camera
		float _mapCenterHeight = _conveyorMaxHeight / 2;
        _mapCenter = new Vector3(
            (levelPath.MatrixSize().x * conveyorPrefab.GameobjectSize.y) / 2,
            _mapCenterHeight,
            (levelPath.MatrixSize().y * conveyorPrefab.GameobjectSize.x) / 2
        );
    }
    private void SetPackageSpawnPosition(GeneratedLevel levelPat) {

		// conveyor prefab data
		Prefab conveyorPrefab = PrefabManager.Instance.GetPrefab("ConveyorBelt");


		_packageSpawnTransform.transform.position = new Vector3(
            levelPat.StartPathPosition().x + (conveyorPrefab.GameobjectSize.x / 2),
            _conveyorMaxHeight + _packageSpawnOffsetHeight,
            levelPat.StartPathPosition().y + (conveyorPrefab.GameobjectSize.y / 2)
        );
    }


    public void StartLevel() {
        if(!_levelLoaded) {
            throw new System.InvalidOperationException("There is no loaded level");
		}

        _spawnLight.gameObject.SetActive(false);

        _spawnLight.gameObject.transform.position = new Vector3(
            _packageSpawnTransform.position.x,
            _conveyorMaxHeight + _lightHeightOffset,
            _packageSpawnTransform.position.z
        );


        _levelState = LevelState.Started;
        StartCoroutine(WaitStartingAnimationAndStart());
    }


	private IEnumerator WaitStartingAnimationAndStart() {

        yield return new WaitForSeconds(2);
        _spawnLight.gameObject.SetActive(true);

        StartCoroutine(DrawTrailIndicator());
        StartCoroutine(WaitAndSpawnPackage());
        


        yield return new WaitForSeconds(2.5f);
        _spawnLight.gameObject.SetActive(false);
    }

    public IEnumerator DrawTrailIndicator() {

        if(_levelState == LevelState.Started) {
            // package prefab data
            string levelMapTrailPoolId = "LevelMapTrail";
            Prefab levelMapTrail = PrefabManager.Instance.GetPrefab("LevelMapTrail");

            /* TRAIL PACKAGE */
            GameObject obj = PrefabManager.Instance.SpawnFromPool(
                levelMapTrailPoolId,
                _packageSpawnTransform.position,
                Quaternion.identity
            );
            TrailIndicator trail = obj.GetComponent<TrailIndicator>();


            trail.Init(
                levelMapTrail.GameobjectSize,
                this
            );
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(DrawTrailIndicator());
        }
            
    }
    private IEnumerator WaitAndSpawnPackage() {

		// package prefab data
		string packagePoolId = "Package";
        Prefab packagePrefab = PrefabManager.Instance.GetPrefab("Package");

        if (_packagesToSpawn > 1) {
            yield return new WaitForSeconds(1);
            GameObject obj = PrefabManager.Instance.SpawnFromPool(
                packagePoolId,
                _packageSpawnTransform.position,
                Quaternion.identity
            );

            Package package = obj.GetComponent<Package>();
            package.Init(
				packagePrefab.GameobjectSize,
                this
            );
            StartCoroutine(WaitAndSpawnPackage());

            _packagesToSpawn--;
            DrawUI();
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
        DrawUI();
        if(IsLevelEnded()) {
            if(IsLevelLose()) {
                _gameController.EndLevelLose();
                _levelState = LevelState.FinishedLose;
            } else {
                _gameController.EndLevelWin();
                _levelState = LevelState.FinishedWin;
            }
        }
    }

    private bool IsLevelEnded() {
        return _loadedLevel.packageToSpawn <= _packagesDestroyed + _packagesDelivered;
    }

    private bool IsLevelLose() {
        return _packagesDestroyed >= _loadedLevel.packageToSpawn / 2;
    }


    private void DrawUI() {
        GameUI.Instance.SetLevelStateDebugValuesUI(
            _packagesToSpawn.ToString(),
            _loadedLevel.packageToSpawn.ToString(),
            _packagesDestroyed.ToString(),
            _packagesDelivered.ToString()
        );
    }
}