using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Course;
using LMS.Core.Repositories;
using LMSGroupOne.Models.MainNavigation;
using LMSGroupOne.Services;
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
        
        
        //private readonly IJTUnitOfWork uow;
        //private readonly UserManager<Person> userManager;
        public AddNavigationController()    //IJTUnitOfWork uow, UserManager<Person> userManager)
        {
            //this.uow = uow;

            //this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            //userManager.GetUsersInRoleAsync("RoleName");
            
        }


        public string OnNew(string path, string id, string type, string name)
        {
            Debug.WriteLine("--------------from add controller-----------");
            Debug.WriteLine("path:"+path);
            Debug.WriteLine("type:"+type);
            Debug.WriteLine("name:"+name);
            Debug.WriteLine("id:"+id);
            Debug.WriteLine("---------------------------------------------");

            int nodeType;
            if (int.TryParse(type, out nodeType))
            {                
                switch (nodeType)
                {
                    case (int)NodeType.student:
                        return TreeNodeFactory.StudentNode(path,id.ToString(), name);
                    case (int)NodeType.teacher:
                        return TreeNodeFactory.TeacherNode(path,id.ToString(), name);
                    case (int)NodeType.file:
                        return TreeNodeFactory.FileNode(path,id.ToString(), name);
                    case (int)NodeType.activity:                        
                        return TreeNodeFactory.ActivityNode(path,id.ToString(), name);
                    case (int)NodeType.module:
                        return TreeNodeFactory.ModuleNode(path, id.ToString(), name);
                    case (int)NodeType.course:                        
                        return TreeNodeFactory.CourseNode(path, id.ToString(), name);
                }
            }            

            
            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = false                                            
                });

            return jsonData;           

        }               

    }

    
}
