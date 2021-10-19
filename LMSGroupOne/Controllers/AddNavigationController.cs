using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMSGroupOne.Models.MainNavigation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class AddNavigationController : Controller
    {
        
        
        private readonly IJTUnitOfWork uow;
        private readonly UserManager<Person> userManager;
        public AddNavigationController(IJTUnitOfWork uow, UserManager<Person> userManager)
        {
            this.uow = uow;

            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            //userManager.GetUsersInRoleAsync("RoleName");
            
        }


        public string OnNew(string path, string id, string type, string name)
        {
            Debug.WriteLine("--------------from add controller-----------");
            Debug.WriteLine("path:"+path);
            Debug.WriteLine("type:"+type);
            Debug.WriteLine("name:"+name);
            Debug.WriteLine("---------------------------------------------");

            int nodeType;
            if (int.TryParse(type, out nodeType))
            {                
                switch (nodeType)
                {
                    case (int)NodeType.student:
                        return StudentNode(path,"new student id", name);
                    case (int)NodeType.teacher:
                        return TeacherNode(path,"new teacher id", name);
                    case (int)NodeType.file:
                        return FileNode(path,"new file id", name);
                    case (int)NodeType.activity:
                        return ActivityNode(path,"new activity id", name);
                    case (int)NodeType.module:
                        return ModuleNode(path, "new module id", name);
                    case (int)NodeType.course:
                        return CourseNode(path, "new course id", name);
                }
            }            

            
            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = false                                            
                });

            return jsonData;           

        }


        private TreeNode MakeNode(string id, string name, NodeType type, NodeType creates, TreeNode[] childNodes)
        {
            return new TreeNode
            {
                Id = id,
                Type = type,
                Name = name,
                Open = false,
                CanCreate = creates,
                Editable = false,
                Nodes = childNodes
            };
        }

        private string StudentNode(string path,string id, string name)
        {            
            TreeNode node = MakeNode(id, name, NodeType.student, NodeType.none, null);            
            return MakeJsonReturnData(true, NodeType.student, path, node);
        }

        private string TeacherNode(string path, string id, string name)
        {                        
            TreeNode node = MakeNode(id, name, NodeType.teacher, NodeType.none, null);
            return MakeJsonReturnData(true, NodeType.teacher, path, node);
        }

        private string FileNode(string path, string id, string name)
        {                        
            TreeNode node = MakeNode(id, name, NodeType.file, NodeType.none, null);
            return MakeJsonReturnData(true, NodeType.file, path, node);
        }

        private string MakeChildId(string path, NodeType type, string parentId)
        {            
            return $"{path}|{type}={parentId}";
        }

        private string ActivityNode(string path, string id, string name)
        {

            TreeNode[] childNodes = new TreeNode[]
            {                
                MakeNode(MakeChildId(path,NodeType.folder,id), "Documents", NodeType.folder, NodeType.file, new TreeNode[]{ })
            };

            TreeNode node = MakeNode(id, name, NodeType.activity, NodeType.none, childNodes);

            return MakeJsonReturnData(true, NodeType.activity, path, node);

        }

        private string ModuleNode(string path, string id, string name)
        {
            TreeNode[] childNodes = new TreeNode[]
            {
                MakeNode(MakeChildId(path,NodeType.folder,id), "Activities", NodeType.folder, NodeType.activity, new TreeNode[]{ }),
                MakeNode(MakeChildId(path,NodeType.folder,id), "Documents", NodeType.folder, NodeType.file, new TreeNode[]{ })
            };

            TreeNode node = MakeNode(id, name, NodeType.module, NodeType.none, childNodes);

            return MakeJsonReturnData(true, NodeType.module, path, node);

        }

        private string CourseNode(string path, string id, string name)
        {
            TreeNode[] childNodes = new TreeNode[]
            {
                MakeNode(MakeChildId(path,NodeType.folder,id), "Modules", NodeType.folder, NodeType.module, new TreeNode[]{ }),
                MakeNode(MakeChildId(path,NodeType.folder,id), "Documents", NodeType.folder, NodeType.file, new TreeNode[]{ }),
                MakeNode(MakeChildId(path,NodeType.folder,id), "Student", NodeType.folder, NodeType.student, new TreeNode[]{ })
            };

            TreeNode node = MakeNode(id, name, NodeType.course, NodeType.none, childNodes);

            return MakeJsonReturnData(true, NodeType.course, path, node);
        }


        private string MakeJsonReturnData(bool success, NodeType nodeType, string path, TreeNode node)
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
