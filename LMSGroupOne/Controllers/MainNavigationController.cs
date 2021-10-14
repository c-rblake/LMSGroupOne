using LMS.Core.Dto;
using LMS.Core.Repositories;
using LMSGroupOne.Models.MainNavigation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
 

namespace LMSGroupOne.Controllers
{
    public class MainNavigationController : Controller
    {
        private readonly IJTUnitOfWork uow;
        public MainNavigationController(IJTUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            
            var model = new TreeNode
            {
                Id = "1",
                Name = "LMS",
                Type = NodeType.root,
                CanCreate = NodeType.course,
                Editable = true,
                Nodes = new TreeNode[]
                { 
                    await Courses("1"),
                    //await Teachers("1"),
                    new TreeNode
                    { 
                        Id="2",
                        Name="literature Search",
                        Type=NodeType.search,
                        CanCreate=NodeType.none,                        
                        Editable=false,
                        Open=false,
                        Nodes=null
                    }
                }
                
            };

            return View(model);
           
        }

        private async Task<TreeNode> Courses(string parentId)
        {
            List<TreeNode> courses = new List<TreeNode>();
            foreach (var item in await uow.CourseRepository.GetTreeData())
            {
                courses.Add(new TreeNode
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = NodeType.course,
                    CanCreate = NodeType.none,
                    Editable = true,
                    Open = false,
                    Nodes = new TreeNode[]
                    {
                        Modules(item.Id, item.Nodes),
                        Documents(item.Id, item.Documents),
                        Students(item.Id, item.Persons)
                    }
                });
            }

            var model = new TreeNode
            {
                Id = parentId,
                Name = "Courses",
                Type = NodeType.folder,
                CanCreate = NodeType.course,
                Editable = true,
                Nodes = courses
            };

            return model;
        }


        private async Task<TreeNode> Teachers(string parentId)
        {
            //List<TreeNode> teachers = new List<TreeNode>();
            //foreach (var item in await uow.TeacherRepository.GetTreeData())
            //{
            //    teachers.Add(new TreeNode
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Type = NodeType.teacher,
            //        CanCreate = NodeType.none,
            //        Editable = true,
            //        Open = false,
            //        Nodes = null                    
            //    });
            //}

            //var model = new TreeNode
            //{
            //    Id = parentId,
            //    Name = "Teachers",
            //    Type = NodeType.folder,
            //    CanCreate = NodeType.teacher,
            //    Editable = true,
            //    Nodes = teachers
            //};

            //return model;
            return null;
        }




        private TreeNode Documents(string parentId, IEnumerable<TreeDataDto> nodes)
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    nodeList.Add(new TreeNode
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Type = NodeType.file,
                        CanCreate = NodeType.none,
                        Editable = true,
                        Open = false,
                        Nodes = null

                    });
                }
            }
            var model = new TreeNode
            {
                Id = parentId,
                Name = "Documents",
                Type = NodeType.folder,
                CanCreate = NodeType.file,
                Editable = true,
                Nodes = nodeList

            };
            return model;
        }


        private TreeNode Students(string parentId, IEnumerable<TreeDataDto> nodes)
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    nodeList.Add(new TreeNode
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Type = NodeType.student,
                        CanCreate = NodeType.none,
                        Editable = true,
                        Open = false,
                        Nodes = null

                    });
                }
            }
            var model = new TreeNode
            {
                Id = parentId,
                Name = "Students",
                Type = NodeType.folder,
                CanCreate = NodeType.student,
                Editable = true,
                Nodes = nodeList
            };
            return model;
        }





        private TreeNode Modules(string parentId, IEnumerable<TreeDataDto> nodes)
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            foreach (var item in nodes)
            {
                nodeList.Add(new TreeNode
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = NodeType.module,
                    CanCreate = NodeType.none,
                    Editable = true,
                    Open = false,
                    Nodes = new TreeNode[]
                    { 
                        Activities(item.Id, item.Nodes),
                        Documents(item.Id, item.Documents)
                    }
                                        
                });
            }
            var model = new TreeNode
            {
                Id = parentId,
                Name = "Modules",
                Type = NodeType.folder,
                CanCreate = NodeType.module,
                Editable = true,
                Nodes = nodeList

            };
            return model;
        }


        


        private TreeNode Activities(string parentId, IEnumerable<TreeDataDto> nodes)
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            if (nodes != null)
            {
                foreach (var item in nodes)
                {
                    nodeList.Add(new TreeNode
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Type = NodeType.activity,
                        CanCreate = NodeType.none,
                        Editable = true,
                        Open = false,
                        Nodes = new TreeNode[] 
                        { 
                            Documents(item.Id, item.Documents)
                        }
                    });
                }
            }

            var model = new TreeNode
            {
                Id = parentId,
                Name = "Activities",
                Type = NodeType.folder,
                CanCreate = NodeType.activity,
                Editable = true,
                Nodes = nodeList

            };
            return model;
        }






      
      

     

     


        
        public string OnDelete(string id, string type)
        {
            Debug.WriteLine($"from controller delete - id:{id}, type:{type}");


            string jsonData = JsonConvert.SerializeObject(
                new 
                {
                    success = true
                });

            return jsonData;
        }


        

        public string OnNew(string id, string type)
        {
            // todo
            Debug.WriteLine($"from controller new - id:{id}, type:{type}");


            string jsonData = JsonConvert.SerializeObject(
                new
                {
                    success = true,
                    id="123",
                    name="kalle",
                    type=type,
                    parentId=id,
                    parentType="folder"
                });

            return jsonData;
        }

        

        public async Task<IActionResult> OnTreeClick(string id, string type)
        {
            Debug.WriteLine($"from controller- id:{id}, type:{type}");

            NodeType result = 0;
            NodeType.TryParse(type, out result);
            
            Debug.WriteLine("---------------------------------------------------");
            switch (result)
            {
                case NodeType.teacher:
                    return Teacher(id);
                case NodeType.student:
                    return Student(id);
                case NodeType.activity:
                    return await Activity(id);
                case NodeType.file:
                    return Document(id);
                case NodeType.module:
                    return await Module(id);
                case NodeType.course:
                    return await Course(id);
                case NodeType.search:
                    return Search(id);

            }

            return new EmptyResult();
        }



        private IActionResult Teacher(string id)
        {
            
            
            var model = new PlaceholderModelView
            {
                Id = id
            };

            return PartialView("Teacher", model);
        }

        private IActionResult Student(string id)
        {
            var model = new PlaceholderModelView
            {
                Id = id
            };

            return PartialView("Student", model);
        }

        private async Task<IActionResult> Activity(string id)
        {
            int aid;
            if (int.TryParse(id, out aid))
            {
                // todo something
            }
            ActivityDto a = await uow.ActivityRepository.GetActivity(aid);


            var model = new ActivityModelView
            {
                Id = a.Id,
                Name=a.Name,
                Description=a.Description,
                StartDate=a.StartDate,
                EndDate=a.EndDate,
                TypeId=a.TypeId,
                TypeName=a.TypeName,
                TypeDescription=a.TypeDescription,
            };

            return PartialView("Activity", model);
        }

        private IActionResult Document(string id)
        {
            var model = new PlaceholderModelView
            {
                Id = id
            };

            return PartialView("Document", model);
        }

        private async Task<IActionResult> Course(string id)
        {
            int cid;
            if (int.TryParse(id, out cid))
            {
                // todo something
            }
            var c = await uow.CourseRepository.GetCourse(cid);


            var model = new CourseModelView
            {
                Id = c.Id,
                Name =c.Name,
                Description=c.Description,
                StartDate=c.StartDate,
                EndDate=c.EndDate
            };

            return PartialView("Course", model);
        }

        private async Task<IActionResult> Module(string id)
        {
            int mid;
            if (int.TryParse(id, out mid))
            {
                // todo something
            }
            var m = await uow.ModuleRepository.GetModule(mid);

            var model = new ModuleModelView
            {
                Id = m.Id,
                Name=m.Name,
                Description=m.Description,
                StartDate=m.StartDate,
                EndDate=m.EndDate
            };

            return PartialView("Module", model);
        }

        private IActionResult Search(string id)
        {
            var model = new PlaceholderModelView
            {
                Id = id
            };

            return PartialView("Search", model);
        }

    }

    
}
