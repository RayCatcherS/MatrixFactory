using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace PT.DebugView {

    using Enumerations;
    

    public class PTDViewNode : Node {
        
        public PTDViewNode(string nodeName, PTNodeType nodeType) {
            NodeName = nodeName;
            NodeType = nodeType;
        }
        private PTNodeType NodeType { get; set; }
        private string NodeName;




        public void Draw() {

            /* TITLE CONTAINER */
            Label nodeNameLabel = new Label(NodeName);
            nodeNameLabel.AddToClassList("node-label");
            titleContainer.Insert(0, nodeNameLabel);



            /* PARENT PORT */
            Port parentPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            parentPort.portName = "parent";
            Color parentPortColor;
            ColorUtility.TryParseHtmlString("#FCA17D", out parentPortColor);
            parentPort.portColor = parentPortColor;
            if(NodeType != PTNodeType.RootNode) {
                inputContainer.Add(parentPort);
            }




            /* CHILD PORTS */
            Color childrenPortColor;
            ColorUtility.TryParseHtmlString("#266DD3", out childrenPortColor);

            /* FORWARD PORT*/
            Port forwardPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            forwardPort.portName = "forward";
            forwardPort.portColor = childrenPortColor;
            

            /* BACK PORT*/
            Port backPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            backPort.portName = "back";
            backPort.portColor = childrenPortColor;
            

            /* RIGHT PORT*/
            Port rightPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            rightPort.portName = "right";
            rightPort.portColor = childrenPortColor;
            

            /* LEFT PORT*/
            Port leftPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            leftPort.portName = "left";
            leftPort.portColor = childrenPortColor;
            
            if(NodeType == PTNodeType.InternalNode) {
                inputContainer.Add(forwardPort);
                inputContainer.Add(backPort);
                inputContainer.Add(rightPort);
                inputContainer.Add(leftPort);
            }


            /* MATRIX PATH PREVIEW */
            Foldout matrixPathPreviwFoldout = new Foldout() {
                text = "Path Previw"
            };

            extensionContainer.Add(matrixPathPreviwFoldout);

            Draw Matrix preview


            // refresh bottom node visual elements
            RefreshExpandedState();
        }
    }
}
