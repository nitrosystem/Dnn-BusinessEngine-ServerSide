using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Common.Collections
{
  public  class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        private readonly Dictionary<T, TreeNode<T>> _children =
                                            new Dictionary<T, TreeNode<T>>();

        public readonly T Item;
        public TreeNode<T> Parent { get; private set; }

        public TreeNode(T item)
        {
            this.Item = item;
        }

        public TreeNode<T> GetChild(T item)
        {
            return this._children[item];
        }

        public void Add(TreeNode<T> node)
        {
            if (node.Parent != null)
            {
                node.Parent._children.Remove(node.Item);
            }

            node.Parent = this;
            this._children.Add(node.Item, node);
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return this._children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get { return this._children.Count; }
        }
    }

}
