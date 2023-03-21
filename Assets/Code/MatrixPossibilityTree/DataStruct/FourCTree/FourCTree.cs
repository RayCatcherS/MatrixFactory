
namespace PT.DataStruct {
    public class FourCTree<T> {
        public FourCTree() {

        }
        private FourCTreeNode<T> _root;
        


        public void InsRoot(T nodeData) {
            _root = new FourCTreeNode<T>(nodeData);
        }
        public FourCTreeNode<T> Root() {
            return _root;
        }

        /// <summary>
        /// Insert new forward Node
        /// </summary>
        /// <param name="node">node to put the new forword child on</param>
        /// <param name="nodeData">node data</param>
        public void Forward(FourCTreeNode<T> node, T nodeData) {
            node.SetForward(nodeData);
        }
        /// <summary>
        /// Insert new back Node
        /// </summary>
        /// <param name="node">node to put the new back child on</param>
        /// <param name="nodeData">node data</param>
        public void Back(FourCTreeNode<T> node, T nodeData) {
            node.SetBack(nodeData);
        }
        /// <summary>
        /// Insert new right Node
        /// </summary>
        /// <param name="node">node to put the new right child on</param>
        /// <param name="nodeData">node data</param>
        public void Right(FourCTreeNode<T> node, T nodeData) {
            node.SetRight(nodeData);
        }
        /// <summary>
        /// Insert new left Node
        /// </summary>
        /// <param name="node">node to put the new left child on</param>
        /// <param name="nodeData">node data</param>
        public void Left(FourCTreeNode<T> node, T nodeData) {
            node.SetLeft(nodeData);
        }


        /// <summary>
        /// check if the forward child is empty
        /// </summary>
        /// <param name="node">the node whose forward child you want to know if it is empty</param>
        /// <returns></returns>
        public bool ForwardIsEmpty(FourCTreeNode<T> node) {
            FourCTreeNode<T> f = node.GetForward();
            bool result;

            if(f != null) {
                result = true;
            } else {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// check if the back child is empty
        /// </summary>
        /// <param name="node">the node whose back child you want to know if it is empty</param>
        /// <returns></returns>
        public bool BackIsEmpty(FourCTreeNode<T> node) {
            FourCTreeNode<T> b = node.GetBack();
            bool result;

            if(b != null) {
                result = true;
            } else {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// check if the right child is empty
        /// </summary>
        /// <param name="node">the node whose rigth child you want to know if it is empty</param>
        /// <returns></returns>
        public bool RightIsEmpty(FourCTreeNode<T> node) {
            FourCTreeNode<T> r = node.GetRight();
            bool result;

            if(r != null) {
                result = true;
            } else {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// check if the left child is empty
        /// </summary>
        /// <param name="node">the node whose left child you want to know if it is empty</param>
        /// <returns></returns>
        public bool LeftIsEmpty(FourCTreeNode<T> node) {
            FourCTreeNode<T> l = node.GetLeft();
            bool result;

            if(l != null) {
                result = true;
            } else {
                result = false;
            }
            return result;
        }




        /// <summary>
        /// Get the parent of the [node]
        /// </summary>
        /// <param name="node">the node whose parent you want to know</param>
        /// <returns></returns>
        public FourCTreeNode<T> Parent(FourCTreeNode<T> node) {

            return node.GetParent();
        }

        /// <summary>
        /// get node item
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public T read(FourCTreeNode<T> node) {
            return node.getItem();
        }
    }


    public class FourCTreeNode<T> {
        public FourCTreeNode(T nodeValue) {
            _nodeItem = nodeValue;
        }
        private FourCTreeNode<T> _parent;

        // child Nodes
        private FourCTreeNode<T> _forward;
        private FourCTreeNode<T> _back;
        private FourCTreeNode<T> _right;
        private FourCTreeNode<T> _left;

        private T _nodeItem;

        /// <summary>
        /// create a new forward node and set the parent node to it
        /// </summary>
        /// <param name="item">data item to set</param>
        public void SetForward(T item) {
            
            FourCTreeNode<T> newForwardNode = new FourCTreeNode<T>(item);
            newForwardNode.LinkParent(this);

            _forward = newForwardNode;
        }
        public FourCTreeNode<T> GetForward() {
            return _forward;
        }
        /// <summary>
        /// create a new back node and set the parent node to it
        /// </summary>
        /// <param name="item">data item to set</param>
        public void SetBack(T item) {

            FourCTreeNode<T> newBackNode = new FourCTreeNode<T>(item);
            newBackNode.LinkParent(this);

            _back = newBackNode;
        }
        public FourCTreeNode<T> GetBack() {
            return _back;
        }
        /// <summary>
        /// create a new right node and set the parent node to it
        /// </summary>
        /// <param name="item">data item to set</param>
        public void SetRight(T item) {

            FourCTreeNode<T> newRightNode = new FourCTreeNode<T>(item);
            newRightNode.LinkParent(this);

            _back = newRightNode;
        }
        public FourCTreeNode<T> GetRight() {
            return _right;
        }
        /// <summary>
        /// create a new left node and set the parent node to it
        /// </summary>
        /// <param name="item">data item to set</param>
        public void SetLeft(T item) {

            FourCTreeNode<T> newLeftNode = new FourCTreeNode<T>(item);
            newLeftNode.LinkParent(this);

            _back = newLeftNode;
        }
        public FourCTreeNode<T> GetLeft() {
            return _left;
        }

        public void LinkParent(FourCTreeNode<T> parent) {
            this._parent = parent;
        }
        public FourCTreeNode<T> GetParent() {
            return this._parent;
        }


        public T getItem() {
            return _nodeItem;
        }
    }
}