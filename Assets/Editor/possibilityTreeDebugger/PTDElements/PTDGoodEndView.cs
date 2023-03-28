using PT.DataStruct;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PT.DebugView {
    public class PTDGoodEndView : Node {
        static public readonly Vector2 defaultSize = new Vector2(175, 190);

        public PTDGoodEndView(PossibilityPathItem nodeItem, Vector2 position) {
            _nodeItem = nodeItem;

            _position = position;

            SetPosition(new Rect(position, Vector2.zero));
            Draw();

        }

        private Vector2 _position;
        public Vector2 position {
            get { return _position; }
        }

        private PossibilityPathItem _nodeItem;
        private PTDPathMatrixView _ptdMatrix;

        public void Draw() {


            /* TITLE CONTAINER */
            Label nodeNameLabel = new Label(_nodeItem.id);
            nodeNameLabel.AddToClassList("pt-node-label");
            titleContainer.Insert(0, nodeNameLabel);


            /* MATRIX PATH PREVIEW */
            Foldout matrixPathPreviwFoldout = new Foldout() {
                text = "Path Previw"
            };

            extensionContainer.Add(matrixPathPreviwFoldout);
            _ptdMatrix = new PTDPathMatrixView(_nodeItem);
            matrixPathPreviwFoldout.Add(_ptdMatrix);


            // refresh bottom node visual elements
            RefreshExpandedState();

        }
    }
}

