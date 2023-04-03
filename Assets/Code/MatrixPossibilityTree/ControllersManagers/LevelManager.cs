using PT.DataStruct;
using PT.Global;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private PrefabManager _prefabManager;


    private readonly int _CBrotationOffset = 90;
    private readonly float _CBGenerationOffsetHeight = 0.25f;


    private Vector3 _mapPositionStart = Vector3.zero;
    private Vector3 _mapCenter = Vector3.zero;


    private ConveyorBelt[,] _conveyorMap;


    [SerializeField] private bool debugPath = false;

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

        InitMap(levels[43]);


        _cameraController.SetCameraTarget(_mapCenter);
    }

    private void InitGameComponents() {
        _prefabManager = gameObject.GetComponent<PrefabManager>();
    }

    private void InitMap(GoodEndPath level) {
        int rows = level.MatrixSize().x;
        int columns = level.MatrixSize().y;
        _conveyorMap = new ConveyorBelt[rows, columns];

        float conveyorMaxHeight = 0;


        List<ConveyorBelt> gOInstantiaded = new List<ConveyorBelt>();
        Vector3 conveyorBlockOffset = Vector3.zero;



        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int pathLength = level.pathElements.Count;
        int heightRangeMin = 0;
        int heightRangeMax = pathLength;


        /* INIT GAMEOBJECT CONVEYOR MAP*/
        // create random conveyor height
        for(int r = rows - 1; r > -1; r--) {

            conveyorBlockOffset = new Vector3(0, 0, conveyorBlockOffset.z);
            for(int c = 0; c < columns; c++) {
                

                GameObject obj = Instantiate(_prefabManager.conveyorBelt.gObject, _mapPositionStart + conveyorBlockOffset, Quaternion.identity);

                ConveyorBelt conveyorBelt = obj.GetComponent<ConveyorBelt>();
                gOInstantiaded.Add(conveyorBelt);
                _conveyorMap[r, c] = conveyorBelt;


                /* SET CONVEYOR RANDOM ROTATION */
                SecureRandom randomRotationSteps = new SecureRandom();
                int translations = randomRotationSteps.Next(0, 3);
                Quaternion gRot = Quaternion.identity;

                for(int i = 0; i < translations; i++) {
                    gRot = Quaternion.Euler(0, gRot.eulerAngles.y + _CBrotationOffset, 0);
                }

                /* SET CONVEYOR RANDOM HEIGHT(in range) */
                System.Random random = new System.Random();
                int conveyorHeight = random.Next(heightRangeMin, heightRangeMax);

                


                /* INIT NEW CONVEYOR*/
                conveyorBelt.InitConveyorBelt(conveyorHeight * _CBGenerationOffsetHeight, gRot);


                // Set the highest conveyor on the map
                if(conveyorMaxHeight < conveyorBelt.conveyorHeight) {
                    conveyorMaxHeight = conveyorHeight;
                }


                // increment column offset
                conveyorBlockOffset = conveyorBlockOffset + new Vector3(_prefabManager.conveyorBelt.gSize.x, 0, 0);
            }

            // increment row offset
            conveyorBlockOffset = conveyorBlockOffset + new Vector3(0, 0, _prefabManager.conveyorBelt.gSize.x);
        }



        /* INIT GOOD END PATH CONVEYOR CORRECT HEIGHT*/
        for(int i = 0; i < level.pathElements.Count; i++) {

            double conveyorHeight;
            
            if(i != 0) {
                System.Random random = new System.Random();
                int randomNextHeight = random.Next(i - 1, i); // the random height has value between the current offset and the previous one

                conveyorHeight = heightRangeMin + (level.pathElements.Count - randomNextHeight) * _CBGenerationOffsetHeight;
            } else {
                conveyorHeight = heightRangeMin + (level.pathElements.Count - i) * _CBGenerationOffsetHeight;
            }
            

            _conveyorMap[level.pathElements[i].pos.x, level.pathElements[i].pos.y]
                .SetConveyorHeight(conveyorHeight);

            // show path
            if(debugPath) {
                _conveyorMap[level.pathElements[i].pos.x, level.pathElements[i].pos.y]
                    .SetDebugTarget(true);
            }
        }



        /* CALCULATE CENTER OF THE MAP */
        // this can be used to center the game camera

        float _mapCenterHeight = conveyorMaxHeight / 2;
        _mapCenter = new Vector3(
            (level.MatrixSize().y * _prefabManager.conveyorBelt.gSize.x) / 2,
            _mapCenterHeight,
            (level.MatrixSize().x * _prefabManager.conveyorBelt.gSize.y) / 2
        );
    }

}
