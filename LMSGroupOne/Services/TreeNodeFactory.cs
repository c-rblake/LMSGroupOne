using LMSGroupOne.Models.MainNavigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Services
{
    public class TreeNodeFactory
    {
        static public string MakeChildId(string path, NodeType type, string parentId)
        {
            return $"{path}|{type}={parentId}";
        }

        static public TreeNode MakeNode(string id, string name, NodeType type, bool isOpen, NodeType creates, bool editable, IEnumerable<TreeNode> childNodes)
        {
            return new TreeNode
            {
                Id = id,
                Type = type,
                Name = name,
                Open = isOpen,
                CanCreate = creates,
                Editable = editable,
                Nodes = childNodes

            };
        }

        static public string StudentNode(string path, string id, string name)
        {
            TreeNode node = TreeNodeFactory.MakeNode(id, name, NodeType.student, false, NodeType.none, false, null);
            return MakeJsonReturnData(true, NodeType.student, path, node);
        }

        static public string TeacherNode(string path, string id, string name)
        {
            TreeNode node = TreeNodeFactory.MakeNode(id, name, NodeType.teacher, false, NodeType.none, false, null);
            return MakeJsonReturnData(true, NodeType.teacher, path, node);
        }

        static public string FileNode(string path, string id, string name)
        {
            TreeNode node = TreeNodeFactory.MakeNode(id, name, NodeType.file, false, NodeType.none, false, null);
            return MakeJsonReturnData(true, NodeType.file, path, node);
        }

        static public string ActivityNode(string path, string id, string name)
        {

            TreeNode[] childNodes = new TreeNode[]
            {
                TreeNodeFactory.MakeNode(TreeNodeFactory.MakeChildId(path,NodeType.folder,id), "Documents", NodeType.folder,false, NodeType.file,false, new TreeNode[]{ })
            };

            TreeNode node = TreeNodeFactory.MakeNode(id, name, NodeType.activity, false, NodeType.none, false, childNodes);

            return MakeJsonReturnData(true, NodeType.activity, path, node);

        }

        static public string ModuleNode(string path, string id, string name)
        {
            TreeNode[] childNodes = new TreeNode[]
            {
                TreeNodeFactory.MakeNode(TreeNodeFactory.MakeChildId(path,NodeType.folder,id), "Activities", NodeType.folder,false, NodeType.activity,false, new TreeNode[]{ }),
                TreeNodeFactory.MakeNode(TreeNodeFactory.MakeChildId(path,NodeType.folder,id), "Documents", NodeType.folder,false, NodeType.file,false, new TreeNode[]{ })
            };

            TreeNode node = TreeNodeFactory.MakeNode(id, name, NodeType.module, false, NodeType.none, false, childNodes);

            return MakeJsonReturnData(true, NodeType.module, path, node);

        }

        static public string CourseNode(string path, string id, string name)
        {
            TreeNode[] childNodes = new TreeNode[]
            {
                TreeNodeFactory.MakeNode(TreeNodeFactory.MakeChildId(path,NodeType.folder,id), "Modules", NodeType.folder,false, NodeType.module,false, new TreeNode[]{ }),
                TreeNodeFactory.MakeNode(TreeNodeFactory.MakeChildId(path,NodeType.folder,id), "Documents", NodeType.folder,false, NodeType.file,false, new TreeNode[]{ }),
                TreeNodeFactory.MakeNode(TreeNodeFactory.MakeChildId(path,NodeType.folder,id), "Student", NodeType.folder,false, NodeType.student,false, new TreeNode[]{ })
            };

            TreeNode node = TreeNodeFactory.MakeNode(id, name, NodeType.course, false, NodeType.none, false, childNodes);

            return MakeJsonReturnData(true, NodeType.course, path, node);
        }


        static private string MakeJsonReturnData(bool success, NodeType nodeType, string path, TreeNode node)
        {
            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = success,
                    type = nodeType,
                    path = path,
                    subTree = node
                });

            return jsonData;
        }

    }
}
