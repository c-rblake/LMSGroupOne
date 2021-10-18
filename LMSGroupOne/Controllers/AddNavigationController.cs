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


        public string OnNew(string id, string type, string name)
        {
            

            int nodeType;
            if (int.TryParse(type, out nodeType))
            {                
                switch (nodeType)
                {
                    case (int)NodeType.student:
                        return StudentNode(id, name);
                    case (int)NodeType.teacher:
                        return TeacherNode(id, name);
                        
                    
                    
                        
                }

            }
            

            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = false                                            
                });

            return jsonData;

          
        }


        private string StudentNode(string id, string name)
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
                    parentId = id,                    
                    subTree = node
                });

            return jsonData;

        }

        private string TeacherNode(string id, string name)
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
                    parentId = id,
                    subTree = node
                });

            return jsonData;

        }


    }
}
