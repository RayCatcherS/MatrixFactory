using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace PT.DebugView {

    using Enumerations;
    public class PTDNodeView : Node {

        static public readonly Vector2 defaultSize = new Vector2(175, 190);


        public PTDNodeView(string nodeName, PTNodeType nodeType, Vector2 position) {
            NodeName = nodeName;
            NodeType = nodeType;

            Draw();

            SetPosition(new Rect(position, Vector2.zero));
            _position = position;
        }
        private Vector2 _position = Vector2.zero;
        public Vector2 position {
            get { return _position; }
        }

        private PTNodeType NodeType { get; set; }
        private string NodeName;

        private Port _parentPort;
        private Port _forwardPort;
        private Port _backPort;
        private Port _rightPort;
        private Port _leftPort;

        public Port parentPort {get {return _parentPort;}}
        public Port forwardPort { get {return _forwardPort; }}
        public Port backPort { get {return _backPort; }}
        public Port rightPort { get {return _rightPort; } }
        public Port leftPort { get {return _leftPort; } }

        private PTDMatrix _ptdMatrix;
        public PTDMatrix ptdMatrix {
            get { return _ptdMatrix; }
        }

        public void Draw() {

            /* TITLE CONTAINER */
            Label nodeNameLabel = new Label(NodeName);
            nodeNameLabel.AddToClassList("pt-node-label");
            titleContainer.Insert(0, nodeNameLabel);



            /* PARENT PORT */
            _parentPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            _parentPort.portName = "parent";
            Color parentPortColor;
            ColorUtility.TryParseHtmlString("#FCA17D", out parentPortColor);
            _parentPort.portColor = parentPortColor;
            if(NodeType != PTNodeType.RootNode) {
                inputContainer.Add(_parentPort);
            }



            /* CHILD PORTS */
            Color childrenPortColor;
            ColorUtility.TryParseHtmlString("#266DD3", out childrenPortColor);

            /* FORWARD PORT*/
            _forwardPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _forwardPort.portName = "forward";
            _forwardPort.portColor = childrenPortColor;
            

            /* BACK PORT*/
            _backPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _backPort.portName = "back";
            _backPort.portColor = childrenPortColor;
            

            /* RIGHT PORT*/
            _rightPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _rightPort.portName = "right";
            _rightPort.portColor = childrenPortColor;


            /* LEFT PORT*/
            _leftPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            _leftPort.portName = "left";
            _leftPort.portColor = childrenPortColor;
            
            if(NodeType == PTNodeType.InternalNode || NodeType == PTNodeType.RootNode) {
                inputContainer.Add(_forwardPort);
                inputContainer.Add(_backPort);
                inputContainer.Add(_rightPort);
                inputContainer.Add(_leftPort);
            }


            /* MATRIX PATH PREVIEW */
            Foldout matrixPathPreviwFoldout = new Foldout() {
                text = "Path Previw"
            };

            extensionContainer.Add(matrixPathPreviwFoldout);
            _ptdMatrix = new PTDMatrix(3, 3);
            matrixPathPreviwFoldout.Add(ptdMatrix);


            // refresh bottom node visual elements
            RefreshExpandedState();
        }
    
        public static Vector2 calculateRelativeNodePosition(PTDNodeView parentNode, NodePort nPort, int treeHeight, int nodeHeight) {

            float xNodePosOffset = 100;
            float yNodePosOffset = 20;

            Vector2 pos = new Vector2(
                parentNode.position.x,
                parentNode.position.y
            );

            float nodeViewHeight = 0;
            float nodeViewWidth = 0;


            /* NODE VIEW SIZE */
            nodeViewHeight = defaultSize.y + parentNode.ptdMatrix.matrixHeigth/*matrix offset*/;
            if(parentNode.ptdMatrix.matrixWidth > defaultSize.x) {
                nodeViewWidth = defaultSize.x + (parentNode.ptdMatrix.matrixWidth/*matrix offset*/ - defaultSize.x) + 25/* node border constant*/;
            } else {
                nodeViewWidth = defaultSize.x;
            }

            float nodeViewHeightPos = 0;
            float nodeViewWidthPos = 0;


            if(nPort == NodePort.forward) {

                //nodeViewHeightPos = nodeViewHeight;

                nodeViewHeightPos = (((0 * nodeViewHeight)) - (0 /* y offset nodes*/));

            } else if(nPort == NodePort.back) {

                //nodeViewHeightPos = nodeViewHeight;
                nodeViewHeightPos = (((1 * nodeViewHeight)) - (0 /* y offset nodes*/));

            } else if(nPort == NodePort.right) {

                //nodeViewHeightPos = nodeViewHeight;
                nodeViewHeightPos = (((2 * nodeViewHeight)) - (0 /* nodeViewHeight * 1.5f y offset nodes*/));

            } else if(nPort == NodePort.left) {

                //nodeViewHeightPos = nodeViewHeight;
                nodeViewHeightPos = (((3 * nodeViewHeight)) - (0 /* y offset nodes*/));
            }


            /* Node x Position*/
            nodeViewWidthPos = nodeViewWidth + xNodePosOffset;

            pos = pos + new Vector2(nodeViewWidthPos, nodeViewHeightPos);

            return pos;
        }
    }

    public enum NodePort {
        forward,
        back,
        right,
        left
    }
}
