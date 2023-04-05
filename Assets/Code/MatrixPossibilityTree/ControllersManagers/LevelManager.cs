using PT.DataStruct;
using PT.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
    private PrefabManager _prefabManager;

    [SerializeField] private Transform _packageSpawnTransform;
    private float _packageSpawnOffsetHeight = 5;

    [SerializeField] private GameObject _spawnLight;
    private float _lightHeightOffset = 2;

    [SerializeField] private bool debugPath = false;

    
    private readonly float _CBGenerationOffsetHeight = 0.25f;


    private Vector3 _mapPositionStart = Vector3.zero;
    private Vector3 _mapCenter = Vector3.zero;


    private ConveyorBelt[,] _conveyorMap;
    private float _conveyorMaxHeight = 0;


    

    public Vector3 MapCenter {
        get { return _mapCenter; }
    }
    private CameraController _cameraController {
        get { return gameObject.GetComponent<GameController>().cameraController; }
    }
    

    void Start() {

        InitGameComponents();


        GlobalPossibilityPath.GeneratePathsFromMatrix();
        List<GoodEndPath> levels = GlobalPossibilityPath.GeneratedGoodEndsPaths;
        GoodEndPath levelPath = levels[43];

        InitMap(levelPath);


        _cameraController.SetCameraTarget(_mapCenter);
        SetPackageSpawnPosition(levelPath);


        StartGame();
    }

    private void InitGameComponents() {
        _prefabManager = gameObject.GetComponent<PrefabManager>();
    }

    private void InitMap(GoodEndPath levelPath) {
        int rows = levelPath.MatrixSize().x;
        int columns = levelPath.MatrixSize().y;
        _conveyorMap = new ConveyorBelt[rows, columns];
        


        List<ConveyorBelt> gOInstantiaded = new List<ConveyorBelt>();
        Vector3 conveyorBlockOffset = Vector3.zero;



        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int pathLength = levelPath.pathElements.Count;
        int heightRangeMin = 0;
        int heightRangeMax = pathLength;


        /* INIT GAMEOBJECT CONVEYOR MAP*/
        // create random conveyor height
        for(int r = 0; r < rows; r++) {

            conveyorBlockOffset = new Vector3(conveyorBlockOffset.x, 0, 0);
            for(int c = 0; c < columns; c++) {
                

                GameObject obj = Instantiate(_prefabManager.conveyorBelt.GetGameobject, _mapPositionStart + conveyorBlockOffset, Quaternion.identity);

                ConveyorBelt conveyorBelt = obj.GetComponent<ConveyorBelt>();
                gOInstantiaded.Add(conveyorBelt);
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
                conveyorBelt.InitConveyorBelt(conveyorUnitHeight * _CBGenerationOffsetHeight, rollerConveyorDirection);


                // Set/Update the highest conveyor on the map
                if(_conveyorMaxHeight < conveyorBelt.RollerConveyorHeight) {
                    _conveyorMaxHeight = conveyorBelt.RollerConveyorHeight;
                }


                // increment column offset
                conveyorBlockOffset = conveyorBlockOffset + new Vector3(0, 0, _prefabManager.conveyorBelt.GameobjectSize.x);
            }

            // increment row offset
            conveyorBlockOffset = conveyorBlockOffset + new Vector3(_prefabManager.conveyorBelt.GameobjectSize.x, 0, 0);
        }



        /* INIT GOOD END PATH CONVEYOR CORRECT HEIGHT*/
        for(int i = 0; i < levelPath.pathElements.Count; i++) {

            double conveyorHeight;
            
            if(i != 0) {
                System.Random random = new System.Random();
                int randomNextHeight = random.Next(i - 1, i + 1); // the random height has value between the current offset and the previous one

                conveyorHeight = heightRangeMin + (levelPath.pathElements.Count - randomNextHeight) * _CBGenerationOffsetHeight;
            } else {
                conveyorHeight = heightRangeMin + (levelPath.pathElements.Count - i) * _CBGenerationOffsetHeight;
            }


            ConveyorBelt pathCurrentConveyor = _conveyorMap[levelPath.pathElements[i].pos.x, levelPath.pathElements[i].pos.y];
            pathCurrentConveyor.SetRollerConveyorHeight(conveyorHeight);

            // Set/Update the highest conveyor on the map
            if(_conveyorMaxHeight < pathCurrentConveyor.RollerConveyorHeight) {
                _conveyorMaxHeight = pathCurrentConveyor.RollerConveyorHeight;
            }

            // show path
            if(debugPath) {
                pathCurrentConveyor.EnableDebugShowPath(true);
            }
        }



        /* CALCULATE CENTER OF THE MAP */
        // this can be used to center the game camera

        float _mapCenterHeight = _conveyorMaxHeight / 2;
        _mapCenter = new Vector3(
            (levelPath.MatrixSize().x * _prefabManager.conveyorBelt.GameobjectSize.y) / 2,
            _mapCenterHeight,
            (levelPath.MatrixSize().y * _prefabManager.conveyorBelt.GameobjectSize.x) / 2
        );
    }

    private void SetPackageSpawnPosition(GoodEndPath levelPat) {
        _packageSpawnTransform.transform.position = new Vector3(
            levelPat.StartPathPosition().x + (_prefabManager.conveyorBelt.GameobjectSize.x / 2),
            _conveyorMaxHeight + _packageSpawnOffsetHeight,
            levelPat.StartPathPosition().y + (_prefabManager.conveyorBelt.GameobjectSize.y / 2)
        );
    }


    private void StartGame() {
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
    }

    private IEnumerator WaitAndSpawnPackage() {


        yield return new WaitForSeconds(2);
        GameObject obj = Instantiate(_prefabManager.package.GetGameobject, _packageSpawnTransform.position, Quaternion.identity);
        Package package = obj.GetComponent<Package>();
        package.Init(_prefabManager.package.GameobjectSize);

        yield return new WaitForSeconds(0.7f);
        StartCoroutine(WaitAndSpawnPackage());
    }
}
