using PT.DataStruct;
using System.Drawing.Drawing2D;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;


namespace PT.DebugView
{
    public class PTDPathMatrixView : VisualElement {   
        public PTDPathMatrixView(PossibilityPathItem possibilityPath) {
            rows = possibilityPath.pathMatrix.GetLength(0);
            columns = possibilityPath.pathMatrix.GetLength(1);
            _possibilityPath = possibilityPath;

            Draw(possibilityPath);
        }
        public PTDPathMatrixView(GoodEndPath goodEndPath) {
            rows = goodEndPath.MatrixSize().x;
            columns = goodEndPath.MatrixSize().y;
            _goodEndPath = goodEndPath;

            Draw(goodEndPath);
        }

        private PossibilityPathItem _possibilityPath;
        private GoodEndPath _goodEndPath;
        private int rows;
        private int columns;
        public int matrixHeigth {
            get { return rows * 30; }
        }
        public int matrixWidth {
            get { return columns * 30; }
        }

        public void Draw(PossibilityPathItem path) {

            /* DRAW MATRIX */

            for(int r = 0; r < rows; r++) {

                VisualElement row = new VisualElement();
                row.AddToClassList("pt-matrix-row");
                // Draw Row
                for(int c = 0; c < columns; c++) {
                    PTDMatrixElementView element = new PTDMatrixElementView(
                        path.pathMatrix[r,c],
                        path
                    );
                    row.Add(element);
                }

                //Add Row to Column
                this.Add(row);
            }
        }
        public void Draw(GoodEndPath path) {

            PossibilityMatrixPathElement[,] _pathMatrix = new PossibilityMatrixPathElement[rows, columns];

            /* INIT MATRIX*/
            for(int r = 0; r < rows; r++) {

                for(int c = 0; c < columns; c++) {

                    _pathMatrix[r, c] = new PossibilityMatrixPathElement(new Vector2Int(r, c));
                }

            }
            for(int i = 0; i < path.pathElements.Count; i++) {


                Vector2Int pos = path.pathElements[i].pos;
                PossibilityMatrixPathElement element = _pathMatrix[
                    pos.x,
                    pos.y
                ];
                element.SetAsTracedPos();



                // set trace direction
                if(path.pathElements[i].pos != path.LastPos()) {
                    Vector2Int direction = path.pathElements[i + 1].pos - path.pathElements[i].pos;

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

            /* DRAW MATRIX */
            for(int r = 0; r < rows; r++) {

                VisualElement row = new VisualElement();
                row.AddToClassList("pt-matrix-row");
                // Draw Row
                for(int c = 0; c < columns; c++) {
                    PTDMatrixElementView element = new PTDMatrixElementView(
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

    public class PTDMatrixElementView : VisualElement {
        public PTDMatrixElementView(PossibilityMatrixPathElement matrixElement, Path path) {
            _matrixElement = matrixElement;
            _path = path;
            Draw ();
        }

        private PossibilityMatrixPathElement _matrixElement;
        Path _path;

        public void Draw() {
            Box matrixBoxElement = new Box();
			matrixBoxElement.AddToClassList("pt-matrix-box");

			if (_matrixElement.tracedPos) {
				matrixBoxElement.RemoveFromClassList("pt-matrix-box");
				matrixBoxElement.AddToClassList ("pt-matrix-box-signed-path");

                if(_matrixElement.tracedMoveDirection == Direction.forward) {

                    matrixBoxElement.Add(new Label("↑"));
                } else if(_matrixElement.tracedMoveDirection == Direction.back) {

                    matrixBoxElement.Add(new Label("↓"));
                } else if(_matrixElement.tracedMoveDirection == Direction.left) {

                    matrixBoxElement.Add(new Label("←"));
                } else if(_matrixElement.tracedMoveDirection == Direction.right) {

                    matrixBoxElement.Add(new Label("→"));
                } else if(_matrixElement.tracedMoveDirection == Direction.stay) {

                    matrixBoxElement.Add(new Label("O"));
                }
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



