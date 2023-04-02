
using System;
using UnityEngine;

namespace PT.DataStruct {
    public class FourCTree<T> {
        public FourCTree() {

        }
        private FourCTreeNode<T> _root;
        private int _treeHeight = 0;
        public int treeHeight {
            get { return _treeHeight; }
        }
        
        public FourCTreeNode<T> Root() {

            if(_root == null) {
                Debug.LogError("Empty tree");
            }
            return _root;
        }
        public FourCTreeNode<T> Forward(FourCTreeNode<T> node) {

            FourCTreeNode<T> value = node.GetForward();

            if (value == null) {
                throw new NullReferenceException();
            }
            return value;
        }
        public FourCTreeNode<T> Back(FourCTreeNode<T> node) {
            FourCTreeNode<T> value = node.GetBack();

            if (value == null) {
                throw new NullReferenceException();
            }
            return value;
        }
        public FourCTreeNode<T> Right(FourCTreeNode<T> node) {
            FourCTreeNode<T> value = node.GetRight();

            if (value == null) {
                throw new NullReferenceException();
            }
            return value;
        }
        public FourCTreeNode<T> Left(FourCTreeNode<T> node) {
            FourCTreeNode<T> value = node.GetLeft();
            if (value == null) {
                throw new NullReferenceException();
            }
            return value;
        }

        public void InsRoot(T nodeData) {
            if (!TreeIsEmpty()) {
                Debug.LogError("Root node already added");
            } else {
                _root = new FourCTreeNode<T>(nodeData, true);
            }
        }
        /// <summary>
        /// Insert new forward Node
        /// </summary>
        /// <param name="node">node to put the new forword child on</param>
        /// <param name="nodeData">node data</param>
        public void InsForward(FourCTreeNode<T> node, T nodeData) {
            if(!ForwardIsEmpty(node)) {

                Debug.LogError("Forward node already added");
            } else {

                node.SetForward(nodeData);
                updateNodeHeight(this.Forward(node));
            }

            
        }
        /// <summary>
        /// Insert new back Node
        /// </summary>
        /// <param name="node">node to put the new back child on</param>
        /// <param name="nodeData">node data</param>
        public void InsBack(FourCTreeNode<T> node, T nodeData) {
            if (!BackIsEmpty(node)) {

                Debug.LogError("Back node already added");
            } else {
                node.SetBack(nodeData);
                updateNodeHeight(this.Back(node));
            }
        }
        /// <summary>
        /// Insert new right Node
        /// </summary>
        /// <param name="node">node to put the new right child on</param>
        /// <param name="nodeData">node data</param>
        public void InsRight(FourCTreeNode<T> node, T nodeData) {
            if (!RightIsEmpty(node)) {
                Debug.LogError("Right node already added");
            } else {
                node.SetRight(nodeData);
                updateNodeHeight(this.Right(node));
            }
            
        }
        /// <summary>
        /// Insert new left Node
        /// </summary>
        /// <param name="node">node to put the new left child on</param>
        /// <param name="nodeData">node data</param>
        public void InsLeft(FourCTreeNode<T> node, T nodeData) {
            if (!LeftIsEmpty(node)) {
                Debug.LogError("Left node already added");
            } else {
                node.SetLeft(nodeData);
                updateNodeHeight(this.Left(node));
            }
        }

        /// <summary>
        /// Call it when the tree inser a new node
        /// </summary>
        /// <param name="node"></param>
        private void updateNodeHeight(FourCTreeNode<T> node) {
            int newNodeHeight = node.nodeHeight;
            if (newNodeHeight > _treeHeight) {
                _treeHeight = newNodeHeight;
            }
        }

        public bool TreeIsEmpty() {
            return (_root == null);
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
                result = false;
            } else {
                result = true;
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
                result = false;
            } else {
                result = true;
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
                result = false;
            } else {
                result = true;
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
                result = false;
            } else {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Check if the node is the root of the tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool isRoot(FourCTreeNode<T> node) {
            return node.isRoot;
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
        public T Read(FourCTreeNode<T> node) {
            if(node == null) {
                throw new NullReferenceException();
            }
            return node.getItem();
        }


        public void VisitTree(FourCTreeNode<T> root, Action<FourCTreeNode<T>, FourCTree<T>> onNodeVisit) {

            if(root == null) {
                Debug.LogError("Empty Tree");
            } else {


                onNodeVisit(root, this);
                if (!ForwardIsEmpty(root)) {
                    VisitTree(root.GetForward(),onNodeVisit);
                }
                if (!BackIsEmpty(root)) {
                    VisitTree(root.GetBack(), onNodeVisit);
                }
                if (!RightIsEmpty(root)) {
                    VisitTree(root.GetRight(), onNodeVisit);
                }
                if (!LeftIsEmpty(root)) {
                    VisitTree(root.GetLeft(), onNodeVisit);
                }
            }
        }

        
    }


    public class FourCTreeNode<T> {
        public FourCTreeNode(T nodeValue, bool isRoot) {
            _isRoot = isRoot;
            _nodeItem = nodeValue;
        }
        private bool _isRoot = false;
        public bool isRoot
        {
            get { return _isRoot; }
        }

        private FourCTreeNode<T> _parent;

        // child Nodes
        private FourCTreeNode<T> _forward;
        private FourCTreeNode<T> _back;
        private FourCTreeNode<T> _right;
        private FourCTreeNode<T> _left;

        private T _nodeItem;

        private int _nodeHeight = 0;

        /// <summary>
        /// Get the tree's node height relative to the tree
        /// </summary>
        public int nodeHeight {
            get { return _nodeHeight; }
        }
        /// <summary>
        /// create a new forward node and set the parent node to it
        /// </summary>
        /// <param name="item">data item to set</param>
        public void SetForward(T item) {
            
            FourCTreeNode<T> newForwardNode = new FourCTreeNode<T>(item, false);
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

            FourCTreeNode<T> newBackNode = new FourCTreeNode<T>(item, false);
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

            FourCTreeNode<T> newRightNode = new FourCTreeNode<T>(item, false);
            newRightNode.LinkParent(this);

            _right = newRightNode;
        }
        public FourCTreeNode<T> GetRight() {
            return _right;
        }
        /// <summary>
        /// create a new left node and set the parent node to it
        /// </summary>
        /// <param name="item">data item to set</param>
        public void SetLeft(T item) {
            FourCTreeNode<T> newLeftNode = new FourCTreeNode<T>(item, false);
            newLeftNode.LinkParent(this);

            _left = newLeftNode;
        }
        public FourCTreeNode<T> GetLeft() {
            return _left;
        }

        public void LinkParent(FourCTreeNode<T> parent) {
            _nodeHeight = parent.nodeHeight + 1;
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