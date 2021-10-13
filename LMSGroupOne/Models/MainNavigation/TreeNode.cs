using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Models.MainNavigation
{
    public class TreeNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public NodeType Type { get; set; }
        public NodeType CanCreate { get; set; }
        public bool Editable { get; set; }
        public bool Open { get; set; }
        public IEnumerable<TreeNode> Nodes { get; set; }
    }
}
