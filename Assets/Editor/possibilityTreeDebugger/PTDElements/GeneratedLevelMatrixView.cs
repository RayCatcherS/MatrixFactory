using PT.DataStruct;
using UnityEngine;
using UnityEngine.UIElements;


namespace PT.DebugView
{
    public class GeneratedLevelMatrixView : VisualElement {   
        public GeneratedLevelMatrixView(GeneratedLevelWithMatrix possibilityPath) {
            rows = possibilityPath.pathMatrix.GetLength(0);
            columns = possibilityPath.pathMatrix.GetLength(1);
            _possibilityPath = possibilityPath;

            Draw(possibilityPath);
        }
        public GeneratedLevelMatrixView(GeneratedLevel generatedLevel) {
            rows = generatedLevel.MatrixSize().x;
            columns = generatedLevel.MatrixSize().y;
            _generatedLevel = generatedLevel;

            Draw(generatedLevel);
        }

        private GeneratedLevelWithMatrix _possibilityPath;
        private GeneratedLevel _generatedLevel;
        private int rows;
        private int columns;
        public int matrixHeigth {
            get { return rows * 30; }
        }
        public int matrixWidth {
            get { return columns * 30; }
        }

        public void Draw(GeneratedLevelWithMatrix path) {

            /* DRAW MATRIX */

            for(int r = 0; r < rows; r++) {

                VisualElement row = new VisualElement();
                row.AddToClassList("pt-matrix-row");
                // Draw Row
                for(int c = 0; c < columns; c++) {
                    GeneratedLevelMatrixElementView element = new GeneratedLevelMatrixElementView(
                        path.pathMatrix[r,c],
                        path
                    );
                    row.Add(element);
                }

                //Add Row to Column
                this.Add(row);
            }
        }
        public void Draw(GeneratedLevel path) {

            GeneratedLevelWithMatrixElement[,] _pathMatrix = new GeneratedLevelWithMatrixElement[rows, columns];

            /* INIT MATRIX*/
            for(int r = 0; r < rows; r++) {

                for(int c = 0; c < columns; c++) {

                    _pathMatrix[r, c] = new GeneratedLevelWithMatrixElement(new Vector2Int(r, c));
                }

            }

            /* INIT PATH */
            for(int i = 0; i < path.PathElements.Count; i++) {


                Vector2Int pos = path.PathElements[i].Pos;
                GeneratedLevelWithMatrixElement element = _pathMatrix[
                    pos.x,
                    pos.y
                ];
                element.SetAsTracedPos();
                element.SetConveyorPlatformType(path.PathElements[i].conveyorBeltPlatformType);



                // set trace direction
                if(path.PathElements[i].Pos != path.LastPathPos()) {
                    Vector2Int direction = path.PathElements[i + 1].Pos - path.PathElements[i].Pos;

                    if(direction == new Vector2Int(-1, 0)) {
                        element.SetTracedMoveDirection(Direction.forward);

                    } else if(direction == new Vector2Int(1, 0)) {
                        element.SetTracedMoveDirection(Direction.back);

                    } else if(direction == new Vector2Int(0, 1)) {
                        element.SetTracedMoveDirection(Direction.right);

                    } else if(direction == new Vector2Int(0, -1)) {
                        element.SetTracedMoveDirection(Direction.left);

                    }

                } else {
                    element.SetTracedMoveDirection(Direction.stay);
                }


            }

            /* BUILD MATRIX */
            for(int r = 0; r < rows; r++) {

                VisualElement row = new VisualElement();
                row.AddToClassList("pt-matrix-row");
                // Draw Row
                for(int c = 0; c < columns; c++) {
                    GeneratedLevelMatrixElementView element = new GeneratedLevelMatrixElementView(
                        _pathMatrix[r, c],
                        path
                    );
                    row.Add(element);
                }

                //Add Row to Column
                this.Add(row);
            }
        }
    }

    public class GeneratedLevelMatrixElementView : VisualElement {
        public GeneratedLevelMatrixElementView(GeneratedLevelWithMatrixElement matrixElement, Path path) {
            _matrixElement = matrixElement;
            _path = path;
            Draw ();
        }

        private GeneratedLevelWithMatrixElement _matrixElement;
        Path _path;

        public void Draw() {
            Box matrixBoxElement = new Box();
			matrixBoxElement.AddToClassList("pt-matrix-box");

			if (_matrixElement.tracedPos) {
				matrixBoxElement.RemoveFromClassList("pt-matrix-box");
				matrixBoxElement.AddToClassList ("pt-matrix-box-signed-path");

                if(_matrixElement.moveDirection == Direction.forward) {

                    matrixBoxElement.Add(new Label("↑"));
                } else if(_matrixElement.moveDirection == Direction.back) {

                    matrixBoxElement.Add(new Label("↓"));
                } else if(_matrixElement.moveDirection == Direction.left) {

                    matrixBoxElement.Add(new Label("←"));
                } else if(_matrixElement.moveDirection == Direction.right) {

                    matrixBoxElement.Add(new Label("→"));
                } else if(_matrixElement.moveDirection == Direction.stay) {

                    matrixBoxElement.Add(new Label("O"));
                }
            }

            // add conveyor platform type
            if(_matrixElement.conveyorPlatformType == ConveyorBelt.PlatformType.ElevatorCannon) {
                matrixBoxElement.Add(new Label("Ele"));
            }

            /* element is end position of the path */
            if (
                _matrixElement.pos.x == _path.EndPathPosition().x &&
                _matrixElement.pos.y == _path.EndPathPosition().y
            ) {
				matrixBoxElement.RemoveFromClassList("pt-matrix-box");
				matrixBoxElement.RemoveFromClassList("pt-matrix-box-signed-path");
				matrixBoxElement.AddToClassList ("pt-matrix-box-end-path");
                matrixBoxElement.Add(new Label ("end"));

            } else if ( /* element is start position of the path */
				_matrixElement.pos.x == _path.StartPathPosition().x &&
				_matrixElement.pos.y == _path.StartPathPosition().y
			) {
                matrixBoxElement.RemoveFromClassList("pt-matrix-box");
				matrixBoxElement.RemoveFromClassList("pt-matrix-box-signed-path");
				matrixBoxElement.AddToClassList("pt-matrix-box-start-path");
				matrixBoxElement.Add (new Label ("start"));

			}


            base.Add(matrixBoxElement);
        }
    }
}



