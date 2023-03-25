using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace PT.DebugView {

    using Enumerations;
    using PT.DataStruct;
    using System;

    public class PTDNodeView : Node {

        static public readonly Vector2 defaultSize = new Vector2(175, 190);


        public PTDNodeView(PossibilityPathItem nodeItem, PTNodeType nodeType, Vector2 position) {
            _nodeItem = nodeItem;
            NodeType = nodeType;

            _position = position;

            SetPosition(new Rect(position, Vector2.zero));
            Draw();
            
        }
        private Vector2 _position;
        public Vector2 position {
            get { return _position; }
        }

        private PTNodeType NodeType { get; set; }
        private PossibilityPathItem _nodeItem;

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

        private PTDPathMatrixView _ptdMatrix;
        public PTDPathMatrixView ptdMatrix {
            get { return _ptdMatrix; }
        }

        public void Draw() {

            /* TITLE CONTAINER */
            Label nodeNameLabel = new Label(_nodeItem.id);
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
            _ptdMatrix = new PTDPathMatrixView(_nodeItem);
            matrixPathPreviwFoldout.Add(ptdMatrix);


            // refresh bottom node visual elements
            RefreshExpandedState();
        }
    
        public static Vector2 calculateRelativeNodeChildPosition(PTDNodeView parentNode, NodePort nPort, int treeHeight, int nodeHeight) {

            double xNodePosOffset = 100;

            Vector2 pos = new Vector2(
                parentNode.position.x,
                parentNode.position.y
            );

            double nodeViewHeight = 0;
            double nodeViewWidth = 0;


            /* NODE VIEW SIZE */
            nodeViewHeight = defaultSize.y + parentNode.ptdMatrix.matrixHeigth/*matrix offset*/;
            if(parentNode.ptdMatrix.matrixWidth > defaultSize.x) {
                nodeViewWidth = defaultSize.x + (parentNode.ptdMatrix.matrixWidth/*matrix offset*/ - defaultSize.x) + 25/* node border constant*/;
            } else {
                nodeViewWidth = defaultSize.x;
            }

            double nodeViewHeightPos = 0;
            double nodeViewWidthPos = 0;


            if(nPort == NodePort.forward) {


                nodeViewHeightPos = (
                    (0 * nodeViewHeight /* node pos*/) - (nodeViewHeight * 1.5f /* y offset nodes*/)
                )

                /* the height size increase in exponential 
                 * from the bottom of the tree to the top. 
                 * The bottom of the tree is (treeHeight - nodeHeight)
                 * 4 is the max degree of nodes
                 */
                * Math.Pow(4, (treeHeight - nodeHeight));


            } else if(nPort == NodePort.back) {


                nodeViewHeightPos = (
                    (1 * nodeViewHeight) - (nodeViewHeight * 1.5f)
                ) * Math.Pow(4, (treeHeight - nodeHeight));

            } else if(nPort == NodePort.right) {


                nodeViewHeightPos = (
                    (2 * nodeViewHeight) - (nodeViewHeight * 1.5f)
                ) * Math.Pow(4, (treeHeight - nodeHeight));

            } else if(nPort == NodePort.left) {


                nodeViewHeightPos = (
                    (3 * nodeViewHeight) - (nodeViewHeight * 1.5f)
                ) * Math.Pow(4, (treeHeight - nodeHeight));
            }



            /* Node x Position*/
            nodeViewWidthPos = nodeViewWidth + xNodePosOffset;

            pos = pos + new Vector2((float)nodeViewWidthPos, (float)nodeViewHeightPos);

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
