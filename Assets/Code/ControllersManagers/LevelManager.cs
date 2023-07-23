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
    private readonly float _conveyorPlatformUnitWorldHeight = 0.25f;
    private Vector3 _mapPositionStart = Vector3.zero;
    private Vector3 _mapCenter = Vector3.zero;
    private ConveyorBelt[,] _conveyorMap;
    private List<ConveyorBelt> _conveyorMapList = new List<ConveyorBelt>();
    private ConveyorBelt _incineratorConveyorBeltLevel;

    private float _conveyorMaxHeight = 0;

    [Header("Delivery Point Generation")]
    private GameObject _deliveryPoint;

    [Header("Package Generation")]
    private List<Package.PackageType> _packagesSequenceToSpawn = new List<Package.PackageType>();
    private int _remainingLevelPackagesToSpawn = 0;
    private int _packagesDestroyed = 0;
    private int _packagesDelivered = 0;


    [Header("Level")]
    [SerializeField] private Transform _packageSpawnTransform;
    private float _packageSpawnOffsetHeight = 5;
    [SerializeField] private GameObject _spawnLight;
    private float _spawnPackageLightHeightOffset = 2.5f;
    [SerializeField] private float _timeBeforeStartingLevelBeforeLight;
    [SerializeField] private float _timeBeforeStartingLevelAfterLight;
    private GeneratedLevel _loadedLevel;
    private LevelState _levelState = LevelState.NotStarted;

    [Header("Level Sounds References")]
    [SerializeField] private AudioClip _levelStartClip;

    public LevelState State {
        get { return _levelState; }
    }
    public Vector3 LevelMapCenter {
        get { return _mapCenter; }
    }

    private LevelInfo _levelInfo;
    [SerializeField] private bool _debugPath = false;
    [SerializeField] private bool _randomPathDirection = true;
    private bool _levelLoaded = false;


    public LevelInfo LevelInfo {

        get {
            if(_levelInfo.Chapter == Chapter.NotSelected || _levelInfo.LevelIndex == -1) {
                throw new System.InvalidOperationException("No Chapter or level selected");
            }
            return _levelInfo;
        }
    }


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


        
        if(LevelInfo.Chapter == Chapter.Chapter1 && LevelInfo.LevelIndex == 0) { // get the tutorial packages sequence

            // get the tutorial packages sequence if the level is the 1 tutorial
            _packagesSequenceToSpawn = _loadedLevel.Tutorial1PackagesSequence;
        } else {
            _packagesSequenceToSpawn = _loadedLevel.PackagesSequence;
        }
            

        _remainingLevelPackagesToSpawn = _loadedLevel.TotalPackageToSpawn;


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
            _packagesSequenceToSpawn = new List<Package.PackageType>();
            _remainingLevelPackagesToSpawn = 0;
            _packagesDestroyed = 0;


            _deliveryPoint.GetComponent<DeliveryPoint>().WipeDeliveryPackages();
            Destroy(_deliveryPoint);

            foreach(ConveyorBelt conveyor in _conveyorMapList) {
                Destroy(conveyor.gameObject);
            }
            _conveyorMapList.Clear();
            _incineratorConveyorBeltLevel = null;


            _levelState = LevelState.NotStarted;
        }
    }

    private void GenerateLevelMap(GeneratedLevel levelPath) {

        InitMap(levelPath);
        GenerateMapConveyors(levelPath);
        GeneratePath(levelPath, _randomPathDirection);
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
    /// Generate the gameobjects conveyors on the map, set the conveyor map(conveyor heights)
    /// and set random conveyor direction
    /// </summary>
    /// <param name="levelPath"></param>
    private void GenerateMapConveyors(GeneratedLevel levelPath) {

        int rows = levelPath.MatrixSize().x;
        int columns = levelPath.MatrixSize().y;
        Vector3 conveyorBaseBlockOffset = Vector3.zero;

        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int conveyorPlatformHeightRangeMin = 0;
        int conveyorPlatformHeightRangeMax = levelPath.StartingLevelHeightPlatform;


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
                //SecureRandom randomPlatformRotationSteps = new SecureRandom();
                System.Random randomPlatformRotationSteps = new System.Random();
                
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
                conveyorBelt.InitConveyorBelt(conveyorPlatformUnitHeight * _conveyorPlatformUnitWorldHeight, conveyorPlatformDirection, ConveyorBelt.PlatformType.Roller);


                // Set/Update the highest conveyor on the map
                if(_conveyorMaxHeight < conveyorBelt.PlatformConveyorHeight) {
                    _conveyorMaxHeight = conveyorBelt.PlatformConveyorHeight;
                }


                // increment column offset
                conveyorBaseBlockOffset = conveyorBaseBlockOffset + new Vector3(0, 0, conveyorPrefab.GameobjectSize.x);
            }

            // increment row offset
            conveyorBaseBlockOffset = conveyorBaseBlockOffset + new Vector3(conveyorPrefab.GameobjectSize.x, 0, 0);
        }
    }

    /// <summary>
    /// /// <summary>
    /// Set map conveyors with correct height for the path
    /// </summary>
    /// <param name="levelPath"> level path to use to initialize the map</param>
    /// <param name="randomPathDirection">by default is true, randomize the direction of the conveyor belts</param>
    private void GeneratePath(GeneratedLevel levelPath, bool randomPathDirection = true) {

        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int conveyorPlatformHeightRangeMin = 0;
        int conveyorPlatformHeightRangeMax = levelPath.StartingLevelHeightPlatform;

        /* INIT GAME PATH CONVEYOR CORRECT HEIGHT*/

        // It represents how many units the height of the next conveyor must be decreased by
        int platformHeightTargetDecrementer = 0;


        for(int i = 0; i < levelPath.PathElements.Count; i++) {

            double conveyorPlatformHeight;

            if(i != 0) {
                System.Random random = new System.Random();
                int randomNextHeight = random.Next(0, levelPath.DefaultPlatformHeightDecrementer + 1); // the random height has value between the current offset and the previous one
                randomNextHeight = 1;

                conveyorPlatformHeight = (conveyorPlatformHeightRangeMin + (conveyorPlatformHeightRangeMax - (platformHeightTargetDecrementer + randomNextHeight))) * _conveyorPlatformUnitWorldHeight;
            } else {
                conveyorPlatformHeight = (conveyorPlatformHeightRangeMin + (conveyorPlatformHeightRangeMax - platformHeightTargetDecrementer)) * _conveyorPlatformUnitWorldHeight;
            }

            


            ConveyorBelt pathCurrentConveyor = _conveyorMap[levelPath.PathElements[i].Pos.x, levelPath.PathElements[i].Pos.y];


            /* INIT NEW CONVEYOR*/
            if(LevelInfo.Chapter == Chapter.Chapter1 && LevelInfo.LevelIndex == 0) { // not spawn the incinerator for the tutorial

                // in the first level of the tutorial, the conveyors are all rollers
                pathCurrentConveyor.SetConveyorType(ConveyorBelt.PlatformType.Roller); 
            } else {
                pathCurrentConveyor.SetConveyorType(levelPath.PathElements[i].conveyorBeltPlatformType);
            }
            
            pathCurrentConveyor.SetPlatformConveyorHeight(conveyorPlatformHeight);

            float safeElevatorTargetHeightOffset = 0.75f;
            pathCurrentConveyor.SetElevatorCannonTargetHeight((levelPath.ElevatorPlatformHeightDecrementer * _conveyorPlatformUnitWorldHeight) + safeElevatorTargetHeightOffset);



            // Set/Update the highest conveyor on the map
            if(_conveyorMaxHeight < pathCurrentConveyor.PlatformConveyorHeight) {
                _conveyorMaxHeight = pathCurrentConveyor.PlatformConveyorHeight;
            }

            // show path for debu use
            if(_debugPath) {
                pathCurrentConveyor.EnableDebugShowPath(true);
            }

            // set correct conveyor direcion for debug use
            if(!randomPathDirection) {
                

                if(pathCurrentConveyor.CurrentConveyorPlatformType == ConveyorBelt.PlatformType.Incinerator) {
                    pathCurrentConveyor.SetPlatformDirection(Direction.right);
                } else {
                    pathCurrentConveyor.SetPlatformDirection(levelPath.PathElements[i].Direction);
                }
            }
            


            if(levelPath.PathElements[i].conveyorBeltPlatformType == ConveyorBelt.PlatformType.ElevatorCannon) {
                //the platform height of the next conveyor will be higher
                platformHeightTargetDecrementer = platformHeightTargetDecrementer - levelPath.ElevatorPlatformHeightDecrementer; 
            } else {
                platformHeightTargetDecrementer++;
            }

            if(levelPath.PathElements[i].conveyorBeltPlatformType == ConveyorBelt.PlatformType.Incinerator) {
                _incineratorConveyorBeltLevel = pathCurrentConveyor;
            }
        }

    }
   
    /// <summary>
    /// Generate the delivery point onto the map
    /// </summary>
    /// <param name="levelPath"></param>
    private void GenerateDeliveryPoint(GeneratedLevel levelPath) {

        // deliveryPoint prefab data
        Prefab deliveryPoint;

        if(_levelInfo.Chapter == Chapter.Chapter1) {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv1");
        } else if(_levelInfo.Chapter == Chapter.Chapter2) {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv2");
        } else if(_levelInfo.Chapter == Chapter.Chapter3) {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv3");
        } else if(_levelInfo.Chapter == Chapter.Chapter4) {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv4");
        } else if(_levelInfo.Chapter == Chapter.Chapter5) {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv5");
        } else if(_levelInfo.Chapter == Chapter.Chapter6) {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv6");
        } else {
            deliveryPoint = PrefabManager.Instance.GetPrefab("DeliveryPointLv1");
        }



        /* SPAWN DELIVERY POINT*/
        Vector3 deliveryPointPos = new Vector3(
            (levelPath.EndPathPosition().x * deliveryPoint.GameobjectSize.x),
            0,
            (levelPath.EndPathPosition().y * deliveryPoint.GameobjectSize.x) + (deliveryPoint.GameobjectSize.x)
        );
        _deliveryPoint = Instantiate(deliveryPoint.GetGameobject, deliveryPointPos, Quaternion.identity);
        _deliveryPoint.GetComponent<DeliveryPoint>().InitDeliveryPoint();
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

    /// <summary>
    /// Use this function to start the loaded level
    /// </summary>
    /// <exception cref="System.InvalidOperationException"></exception>
    public void StartLevel() {
        if(!_levelLoaded) {
            throw new System.InvalidOperationException("There is no loaded level");
		}

        _spawnLight.gameObject.SetActive(false);

        _spawnLight.gameObject.transform.position = new Vector3(
            _packageSpawnTransform.position.x,
            _conveyorMapList[0].PlatformConveyorHeight + _spawnPackageLightHeightOffset,
            _packageSpawnTransform.position.z
        );


        _levelState = LevelState.Started;
        StartCoroutine(WaitStartingAnimationAndStart());
        DrawUI();
    }


	private IEnumerator WaitStartingAnimationAndStart() {

        yield return new WaitForSeconds(_timeBeforeStartingLevelBeforeLight);
        _spawnLight.gameObject.SetActive(true);
        levelStartSound();

        StartCoroutine(DrawTrailIndicator());

		yield return new WaitForSeconds(_timeBeforeStartingLevelAfterLight);
		StartCoroutine(WaitAndSpawnPackage());
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
            yield return new WaitForSeconds(0.28f);
            StartCoroutine(DrawTrailIndicator());
        }
            
    }
    private IEnumerator WaitAndSpawnPackage() {

		// package prefab data
		string packagePoolId = "Package";
        Prefab packagePrefab = PrefabManager.Instance.GetPrefab(packagePoolId);

        if (_remainingLevelPackagesToSpawn > 1) {
            yield return new WaitForSeconds(1);
            GameObject obj = PrefabManager.Instance.SpawnFromPool(
                packagePoolId,
                _packageSpawnTransform.position,
                Quaternion.identity
            );

            Package package = obj.GetComponent<Package>();

            Package.PackageType pType = _packagesSequenceToSpawn[_loadedLevel.TotalPackageToSpawn - _remainingLevelPackagesToSpawn];
            package.Init(
				packagePrefab.GameobjectSize,
                this,
                pType
            );
            StartCoroutine(WaitAndSpawnPackage());

            _remainingLevelPackagesToSpawn--;
            DrawUI();
        }
    }

    public void PackageDestroyedEvent() {
        _packagesDestroyed++;
        EvaluateEndLevelStatus();
    }

    public void DeliveryPointDamage(int damage) {

        int _packagesDeliveredTemp = _packagesDelivered;


        if(_packagesDelivered - damage < 0) {
            _packagesDelivered = 0;
        } else {
            _packagesDelivered = _packagesDelivered - damage;
        }

        int packageToDestroy = _packagesDeliveredTemp - _packagesDelivered;
        _packagesDestroyed = _packagesDestroyed + packageToDestroy;


        for(int i = 0; i < packageToDestroy; i++) {
            _deliveryPoint.GetComponent<DeliveryPoint>().VisualPackageAction(DeliveryPoint.Action.Decrement);
        }


        EvaluateEndLevelStatus();
    }

    public void PackageDeliveredEvent() {
        _packagesDelivered++;
        EvaluateEndLevelStatus();
        _deliveryPoint.GetComponent<DeliveryPoint>().VisualPackageAction(DeliveryPoint.Action.Increment);
    }

    private void EvaluateEndLevelStatus() {
        DrawUI();
        if(IsLevelEnded()) {
            if(IsLevelLose()) {
                _gameController.EndLevelLose();
                _levelState = LevelState.FinishedLose;


                _deliveryPoint.GetComponent<DeliveryPoint>().ShipmentFailed();

            } else {
                _gameController.EndLevelWin();
                _levelState = LevelState.FinishedWin;

                _deliveryPoint.GetComponent<DeliveryPoint>().SuccessfulShipment();
            }
        }
    }

    private bool IsLevelEnded() {
        return _loadedLevel.TotalPackageToSpawn <= _packagesDestroyed + _packagesDelivered;
    }

    private bool IsLevelLose() {
        return _packagesDelivered < _loadedLevel.NumberOfPackageToWin;
    }


    public void DrawUI() {
        GameUI.Instance.SetLevelStateDebugValuesUI(
            _remainingLevelPackagesToSpawn.ToString(),
            _loadedLevel.TotalPackageToSpawn.ToString(),
            _packagesDestroyed.ToString(),
            _packagesDelivered.ToString(),
            _loadedLevel.NumberOfPackageToWin.ToString()
        );


        if(_packagesDelivered >= _loadedLevel.NumberOfPackageToWin) {

            GameUI.Instance.DrawSufficientPackages(true);
        } else {
            GameUI.Instance.DrawSufficientPackages(false);
        }
    }

    public void SetEnableLevelIncinerator(bool value) {
        if(_incineratorConveyorBeltLevel != null) {
            _incineratorConveyorBeltLevel.SetEnableIncineratorPlatformTrigger(value);
        }
    }

    private void levelStartSound() {
        GameObject audioSource = PrefabManager.Instance.SpawnFromPool("AudioSource", gameObject.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = _levelStartClip;
        audioSource.GetComponent<AudioSource>().Play();
    }
}
