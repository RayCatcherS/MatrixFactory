using PT.DataStruct;
using System.Drawing.Drawing2D;
using UnityEngine.UIElements;


namespace PT.DebugView
{
    public class PTDPathMatrixView : VisualElement
    {   
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
                    PTDMatrixElement element = new PTDMatrixElement(
                        _nodeItem.pathMatrix[r,c]
                    );
                    row.Add(element);
                }

                //Add Row to Column
                this.Add(row);
            }
        }
    }

    public class PTDMatrixElement : VisualElement {
        public PTDMatrixElement(PathMatrixElement element) {
            _element = element;
            Draw();
        }

        private PathMatrixElement _element;

        public void Draw()
        {
            Box matrixBoxElement = new Box();

            matrixBoxElement.Add(new Label(_element.markedPos.ToString()));

            matrixBoxElement.AddToClassList("pt-matrix-box");
            base.Add(matrixBoxElement);
        }
    }
}



