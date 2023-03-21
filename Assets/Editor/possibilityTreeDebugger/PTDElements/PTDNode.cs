using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace PT.DebugView {

    using Enumerations;
    

    public class PTDNodeView : Node {
        
        public PTDNodeView(string nodeName, PTNodeType nodeType, Vector2 position) {
            NodeName = nodeName;
            NodeType = nodeType;

            Draw();

            SetPosition(new Rect(position, Vector2.zero));
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
            PTDMatrix ptdMatrix = new PTDMatrix(3, 3);
            matrixPathPreviwFoldout.Add(ptdMatrix);


            // refresh bottom node visual elements
            RefreshExpandedState();
        }
    }
}
