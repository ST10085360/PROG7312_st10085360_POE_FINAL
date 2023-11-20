using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookSoek
{
    class TreeNode
    {
        public int Level { get; set; }
        public string Book { get; set; }

        public List<TreeNode> Children { get; set; }

        public TreeNode Parent { get; set; }

        public TreeNode(int level, string book) 
        { 
            Level = level;
            Book = book;
            Children = new List<TreeNode>();
            Parent = null;
        }

        public void AddChild(TreeNode child)
        {
            child.Parent = this; // Set the Parent property when adding a child
            Children.Add(child);
        }
    }
}
