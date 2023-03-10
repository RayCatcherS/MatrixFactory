using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        

        public void Draw()
        {
            /* DRAW MATRIX */
            for(int i = 0; i < columns; i++)
            {
                PTDMatrixElement element = new PTDMatrixElement();
                this.Add(element);
            }
        }
    }

    public class PTDMatrixElement : VisualElement
    {
        public PTDMatrixElement()
        {
            Draw();
        }

        public void Draw()
        {
            Box matrixBoxElement = new Box();
            matrixBoxElement.AddToClassList("pt-matrix-box");
            base.Add(matrixBoxElement);
        }
    }
}



