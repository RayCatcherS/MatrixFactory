using PT.DataStruct;
using PT.Global;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private PrefabManager _prefabManager;


    private readonly int _rotationOffset = 90;
    private readonly float _heightOffset = 0.25f;


    private Vector3 _mapPositionStart = Vector3.zero;
    private Vector3 _mapCenter = Vector3.zero;
    private Vector3 _levelSpawn = Vector3.zero;


    private ConveyorBelt[,] _conveyorMap;
    

    public Vector3 MapCenter {
        get { return _mapCenter; }
    }
    
    void Start() {
        InitGameComponents();

        GlobalPossibilityPath.GeneratePathsFromMatrix();
        List<GoodEndPath> levels = GlobalPossibilityPath.GeneratedGoodEndsPaths;

        InitMap(levels[0]);


        Camera.main.gameObject.GetComponent<CameraController>().SetCameraTarget(_mapCenter);
    }

    private void InitGameComponents() {
        _prefabManager = gameObject.GetComponent<PrefabManager>();
    }

    private void InitMap(GoodEndPath level) {
        int rows = level.MatrixSize().x;
        int columns = level.MatrixSize().y;
        _conveyorMap = new ConveyorBelt[rows, columns];


        List<ConveyorBelt> gOInstantiaded = new List<ConveyorBelt>();
        Vector3 conveyorBlockOffset = Vector3.zero;



        /* CALCULATE HEIGHT RANGE OF CONVEYORS */
        // range with which to calculate random heights of the conveyors on the map
        int pathLength = level.pathElements.Count;
        double heightRangeMin = 0.1f;
        double heightRangeMax = (pathLength * _heightOffset);


        /* INIT GAMEOBJECT CONVEYOR MAP*/
        // create random conveyor height
        for(int r = 0; r < rows; r++) {

            conveyorBlockOffset = new Vector3(0, (float) -heightRangeMax, conveyorBlockOffset.z);
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
                    gRot = Quaternion.Euler(0, gRot.eulerAngles.y + _rotationOffset, 0);
                }

                /* SET CONVEYOR RANDOM HEIGHT(in range) */
                System.Random random = new System.Random();
                double conveyorHeight = (random.NextDouble() * (heightRangeMax - heightRangeMin) + heightRangeMin);

                /* INIT NEW CONVEYOR*/
                conveyorBelt.InitConveyorBelt(conveyorHeight, gRot);


                // increment column offset
                conveyorBlockOffset = conveyorBlockOffset + new Vector3(_prefabManager.conveyorBelt.gSize.x, 0, 0);
            }

            // increment row offset
            conveyorBlockOffset = conveyorBlockOffset + new Vector3(0, 0, _prefabManager.conveyorBelt.gSize.x);
        }



        /* INIT GOOD END PATH CONVEYOR CORRECT HEIGHT*/
        for(int i = 0; i < level.pathElements.Count; i++) {

            double conveyorHeight = heightRangeMin + (i * _heightOffset);

            _conveyorMap[level.pathElements[i].pos.x, level.pathElements[i].pos.y]
                .SetConveyorHeight(conveyorHeight);
        }



        /* CALCULATE CENTER OF THE MAP */
        // this can be used to center the game camera
        _mapCenter = new Vector3(
            (level.MatrixSize().y * _prefabManager.conveyorBelt.gSize.y) / 2,
            0,
            (level.MatrixSize().x * _prefabManager.conveyorBelt.gSize.x) / 2
        );
    }

}
