using PT.DataStruct;
using System.Drawing.Drawing2D;
using UnityEngine;
using UnityEngine.UIElements;


namespace PT.DebugView
{
    public class PTDPathMatrixView : VisualElement {   
        public PTDPathMatrixView(PossibilityPathItem nodeItem) {
            rows = nodeItem.pathMatrix.GetLength(0);
            columns = nodeItem.pathMatrix.GetLength(1);
            _nodeItem = nodeItem;

            Draw();
        }
        private PossibilityPathItem _nodeItem;
        private int rows;
        private int columns;
        public int matrixHeigth {
            get { return rows * 30; }
        }
        public int matrixWidth {
            get { return columns * 30; }
        }

        public void Draw() {   

            /* DRAW MATRIX */

            for(int r = 0; r < rows; r++) {

                VisualElement row = new VisualElement();
                row.AddToClassList("pt-matrix-row");
                // Draw Row
                for(int c = 0; c < columns; c++) {
                    PTDMatrixElementView element = new PTDMatrixElementView(
                        _nodeItem.pathMatrix[r,c],
                        _nodeItem
                    );
                    row.Add(element);
                }

                //Add Row to Column
                this.Add(row);
            }
        }
    }

    public class PTDMatrixElementView : VisualElement {
        public PTDMatrixElementView(PathMatrixElement matrixElement, PossibilityPathItem nodeItem) {
            _matrixElement = matrixElement;
            _nodeItem = nodeItem;
            Draw ();
        }

        private PathMatrixElement _matrixElement;
        PossibilityPathItem _nodeItem;

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
                _matrixElement.pos.x == _nodeItem.endPathPosition.x &&
                _matrixElement.pos.y == _nodeItem.endPathPosition.y
            ) {
				matrixBoxElement.RemoveFromClassList("pt-matrix-box");
				matrixBoxElement.RemoveFromClassList("pt-matrix-box-signed-path");
				matrixBoxElement.AddToClassList ("pt-matrix-box-end-path");
                matrixBoxElement.Add(new Label ("end"));

            } else if ( /* element is start position of the path */
				_matrixElement.pos.x == _nodeItem.startPathPosition.x &&
				_matrixElement.pos.y == _nodeItem.startPathPosition.y
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



