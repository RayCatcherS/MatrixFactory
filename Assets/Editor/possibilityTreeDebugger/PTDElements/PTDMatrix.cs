using UnityEngine.UIElements;


namespace PT.DebugView
{
    public class PTDMatrix : VisualElement
    {   
        public PTDMatrix(int matrixRows, int matrixColumns)
        {
            rows = matrixRows;
            columns = matrixColumns;

            Draw();
        }
        private int rows;
        private int columns;
        public int matrixHeigth {
            get { return rows * 30; }
        }
        public int matrixWidth {
            get { return columns * 30; }
        }

        public void Draw()
        {   

            /* DRAW MATRIX */

            for(int j = 0; j < rows; j++) {

                VisualElement row = new VisualElement();
                row.AddToClassList("pt-matrix-row");
                // Draw Row
                for(int i = 0; i < columns; i++) {
                    PTDMatrixElement element = new PTDMatrixElement(new Label(i.ToString()));
                    row.Add(element);
                }

                //Add Row to Column
                this.Add(row);
            }
        }
    }

    public class PTDMatrixElement : VisualElement
    {
        public PTDMatrixElement(VisualElement boxChild = null)
        {
            _boxChild = boxChild;
            Draw();
        }
        private VisualElement _boxChild;

        public void Draw()
        {
            Box matrixBoxElement = new Box();
            if(_boxChild != null) {
                matrixBoxElement.Add(_boxChild);
            }            
            matrixBoxElement.AddToClassList("pt-matrix-box");
            base.Add(matrixBoxElement);
        }
    }
}



