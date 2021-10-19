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



                }

            }
            

            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = false                                            
                });

            return jsonData;

          
        }


        private string StudentNode(string path,string id, string name)
        {

            TreeNode node = new TreeNode
            {
                Id = id,
                Type = NodeType.student,
                Name = name,
                Open = false,
                CanCreate = NodeType.none,
                Editable = false,
                Nodes = null
            };
            
            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = true,                    
                    type = NodeType.student,
                    path = path,                    
                    subTree = node
                });

            return jsonData;

        }

        private string TeacherNode(string path, string id, string name)
        {

            TreeNode node = new TreeNode
            {
                Id = id,
                Type = NodeType.teacher,
                Name = name,
                Open = false,
                CanCreate = NodeType.none,
                Editable = false,
                Nodes = null
            };

            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = true,
                    type = NodeType.teacher,
                    path = path,
                    subTree = node
                });

            return jsonData;

        }

        private string FileNode(string path, string id, string name)
        {

            TreeNode node = new TreeNode
            {
                Id = id,
                Type = NodeType.file,
                Name = name,
                Open = false,
                CanCreate = NodeType.none,
                Editable = false,
                Nodes = null
            };

            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = true,
                    type = NodeType.file,
                    path = path,
                    subTree = node
                });

            return jsonData;

        }


        private string ActivityNode(string path, string id, string name)
        {

            TreeNode node = new TreeNode
            {
                Id = id,
                Type = NodeType.activity,
                Name = name,
                Open = false,
                CanCreate = NodeType.none,
                Editable = false,
                Nodes = new TreeNode[]
                { 
                    new TreeNode
                    {
                        Id = path+"|"+NodeType.folder+"="+id,
                        Type = NodeType.folder,
                        Name = "Documents",
                        Open = false,
                        CanCreate = NodeType.file,
                        Editable = false,
                        Nodes=new TreeNode[]
                        { 
                        
                        }
                    }
                }
            };

            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = true,
                    type = NodeType.activity,
                    path = path,
                    subTree = node
                });

            return jsonData;

        }








        private string ModuleNode(string path, string id, string name)
        {

            TreeNode node = new TreeNode
            {
                Id = id,
                Type = NodeType.module,
                Name = name,
                Open = false,
                CanCreate = NodeType.none,
                Editable = false,
                Nodes = new TreeNode[]
                {
                    new TreeNode
                    {
                        Id = path+"|"+NodeType.folder+"="+id,
                        Type = NodeType.folder,
                        Name = "Activities",
                        Open = false,
                        CanCreate = NodeType.activity,
                        Editable = false,
                        Nodes=new TreeNode[]
                        {

                        }
                    },
                    new TreeNode
                    {
                        Id = path+"|"+NodeType.folder+"="+id,
                        Type = NodeType.folder,
                        Name = "Documents",
                        Open = false,
                        CanCreate = NodeType.file,
                        Editable = false,
                        Nodes=new TreeNode[]
                        {

                        }
                    }
                }
            };

            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = true,
                    type = NodeType.module,
                    path = path,
                    subTree = node
                });

            return jsonData;

        }


    }
}
