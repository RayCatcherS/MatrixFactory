using PT.DataStruct;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PT.DebugView {
    public class PTDGoodEndView : Node {
        static public readonly Vector2 defaultSize = new Vector2(175, 190);

        
        public PTDGoodEndView(GeneratedLevel goodEndPathItem, Vector2 position, int index) {
            _goodPathItem = goodEndPathItem;

            _position = position;

            _indexLevel = index;

            SetPosition(new Rect(position, Vector2.zero));
            Draw();

        }

        private Vector2 _position;
        public Vector2 position {
            get { return _position; }
        }

        private PTDPathMatrixView _ptdMatrix;
        private GeneratedLevel _goodPathItem;

        private int _indexLevel;

        public void Draw() {


            /* TITLE CONTAINER */
            Label nodeNameLabel = new Label(
                "Position Path:\n" + _goodPathItem.id()
            );

            nodeNameLabel.AddToClassList("pt-node-label");
            titleContainer.Insert(0, nodeNameLabel);


            Label nodeInfo = new Label(
               "Level: " + _indexLevel + "\nPath Score: " + _goodPathItem.score
            );
            
            inputContainer.Add(nodeInfo);

            /* MATRIX PATH PREVIEW */
            Foldout matrixPathPreviwFoldout = new Foldout() {
                text = "Path Previw"
            };

            extensionContainer.Add(matrixPathPreviwFoldout);
            _ptdMatrix = new PTDPathMatrixView(_goodPathItem);
            matrixPathPreviwFoldout.Add(_ptdMatrix);


            // refresh bottom node visual elements
            RefreshExpandedState();

        }
    }
}

