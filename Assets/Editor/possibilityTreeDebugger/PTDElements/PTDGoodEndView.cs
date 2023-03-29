using PT.DataStruct;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PT.DebugView {
    public class PTDGoodEndView : Node {
        static public readonly Vector2 defaultSize = new Vector2(175, 190);

        
        public PTDGoodEndView(GoodEndPath goodEndPathItem, Vector2 position) {
            _goodPathItem = goodEndPathItem;

            _position = position;

            SetPosition(new Rect(position, Vector2.zero));
            Draw();

        }

        private Vector2 _position;
        public Vector2 position {
            get { return _position; }
        }

        private PTDPathMatrixView _ptdMatrix;
        private GoodEndPath _goodPathItem;

        public void Draw() {


            /* TITLE CONTAINER */
            Label nodeNameLabel = new Label(
                "Position Path:\n" + _goodPathItem.id()
            );

            nodeNameLabel.AddToClassList("pt-node-label");
            titleContainer.Insert(0, nodeNameLabel);


            Label nodeInfo = new Label(
               "Path Score: " + _goodPathItem.score
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

