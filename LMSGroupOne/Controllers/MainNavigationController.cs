using LMS.Core.Dto;
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
    public class MainNavigationController : Controller
    {
        static int nextId;
        private readonly IJTUnitOfWork uow;
        private readonly UserManager<Person> userManager;
        public MainNavigationController(IJTUnitOfWork uow, UserManager<Person> userManager)
        {
            this.uow = uow;

            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            //userManager.GetUsersInRoleAsync("RoleName");
            //nextId = 0;

        }
         
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            

            if (User.IsInRole("Teacher"))
            {
                var model = new TreeNode
                {
                    Id = "root",
                    Name = "LMS(Teacher)",
                    Type = NodeType.root,
                    CanCreate = NodeType.none,
                    Editable = true,
                    Nodes = new TreeNode[]
                    {
                        await TeacherCourses("root"),
                        await Teachers("root"),
                        new TreeNode
                        {
                            Id="root",
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
            else if(User.IsInRole("Student")) 
            {
                string userId=userManager.GetUserId(User);
                Debug.WriteLine("user id:"+userId);

                var model = new TreeNode
                {
                    Id = "root",
                    Name = "LMS(Student)",
                    Type = NodeType.root,
                    CanCreate = NodeType.none,
                    Editable = false,
                    Nodes = new TreeNode[]
                    {
                        await StudentCourses("root", userId),
                        await Teachers("root"),
                        new TreeNode
                        {
                            Id="root",
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
                        
            return null;
           
        }

        private async Task<TreeNode> StudentCourses(string path, string studentId)
        {
            List<TreeNode> courses = new List<TreeNode>();
            foreach (var item in await uow.CourseRepository.GetTreeDataForStudent(studentId))
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
                        Modules(item.Id, item.Nodes, path+"|"+NodeType.course+"="+item.Id),
                        Documents(item.Id, item.Documents, path+"|"+NodeType.course+"="+item.Id, false),
                        Students(item.Id, item.Persons, path+"|"+NodeType.course+"="+item.Id)
                    }
                });
            }

            var model = new TreeNode
            {
                Id = path,
                Name = "Courses",
                Type = NodeType.folder,
                CanCreate = NodeType.none,
                Editable = true,
                Nodes = courses
            };

            return model;
        }


        private async Task<TreeNode> TeacherCourses(string path)
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
                        Modules(item.Id, item.Nodes, path+"|"+NodeType.course+"="+item.Id),
                        Documents(item.Id, item.Documents, path+"|"+NodeType.course+"="+item.Id, true),
                        Students(item.Id, item.Persons, path+"|"+NodeType.course+"="+item.Id)
                    }
                });
            }

            var model = new TreeNode
            {
                Id = path,
                Name = "Courses",
                Type = NodeType.folder,
                CanCreate = NodeType.course,
                Editable = true,
                Nodes = courses
            };

            return model;
        }


        private async Task<TreeNode> Teachers(string path)
        {
            var persons=await userManager.GetUsersInRoleAsync("Teacher");
            List<TreeNode> teachers = new List<TreeNode>();

            bool isTeacher=User.IsInRole("Teacher");

            foreach (var item in persons)
            {
                teachers.Add(new TreeNode
                {
                    Id = item.Id,
                    Name = $"{item.FirstName} {item.LastName}",
                    Type = NodeType.teacher,
                    CanCreate = NodeType.none,
                    Editable = true,
                    Open = false,
                    Nodes = null                    
                });
            }

            var model = new TreeNode
            {
                Id = path,
                Name = "Teachers",
                Type = NodeType.folder,
                CanCreate = isTeacher?NodeType.teacher:NodeType.none,
                Editable = true,
                Open=false,
                Nodes = teachers                
            };

            return model;
            
        }

        




        private TreeNode Documents(string parentId, IEnumerable<TreeDataDto> nodes, string path, bool canAdd)
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
                Id = path,
                Name = "Documents",
                Type = NodeType.folder,
                CanCreate = canAdd?NodeType.file:NodeType.none,
                Editable = true,
                Nodes = nodeList

            };
            return model;
        }


        private TreeNode Students(string parentId, IEnumerable<TreeDataDto> nodes, string path)
        {
            bool isTeacher = User.IsInRole("Teacher");

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
                Id = path,
                Name = "Students",
                Type = NodeType.folder,
                CanCreate = isTeacher?NodeType.student:NodeType.none,
                Editable = true,
                Nodes = nodeList
            };
            return model;
        }





        private TreeNode Modules(string parentId, IEnumerable<TreeDataDto> nodes, string path)
        {
            bool isTeacher = User.IsInRole("Teacher");

            List<TreeNode> nodeList = new List<TreeNode>();
            if (nodes != null)
            {
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
                        Activities(item.Id, item.Nodes, path+"|"+NodeType.module+"="+item.Id),
                        Documents(item.Id, item.Documents, path+"|"+NodeType.module+"="+item.Id, isTeacher)
                        }

                    });
                }
            }
            var model = new TreeNode
            {
                Id = path,
                Name = "Modules",
                Type = NodeType.folder,
                CanCreate = isTeacher?NodeType.module:NodeType.none,
                Editable = true,
                Nodes = nodeList

            };
            return model;
        }


        


        private TreeNode Activities(string parentId, IEnumerable<TreeDataDto> nodes, string path)
        {
            bool isTeacher = User.IsInRole("Teacher");

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
                            Documents(item.Id, item.Documents, path+"|"+NodeType.activity+"="+item.Id, true)
                        }
                    });
                }
            }

            var model = new TreeNode
            {
                Id = path,
                Name = "Activities",
                Type = NodeType.folder,
                CanCreate = isTeacher?NodeType.activity:NodeType.none,
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


        

       


        // the welcomepage when the page is loaded
        public async Task<IActionResult> OnHome(string type)
        {
            var user=await userManager.GetUserAsync(User);

            if (User.IsInRole("Teacher"))
            {
                
                var model = new TeacherHomeModelView
                {
                    Id = user.Id,
                    Name=$"{user.FirstName} {user.LastName}",
                    Email=user.Email
                };

                return PartialView("TeacherHome", model);
                
            }
            else if (User.IsInRole("Student"))
            {
                
                var model = new StudentHomeModelView
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email                    
                };

                return PartialView("StudentHome", model);                
            }

            
            return new EmptyResult();
        }



        public async Task<IActionResult> OnTreeClick(string id, string type)
        {
            
            NodeType result = 0;
            NodeType.TryParse(type, out result);            
            
            switch (result)
            {
                case NodeType.teacher:
                    return await Teacher(id);
                case NodeType.student:
                    return await Student(id);
                case NodeType.activity:
                    return await Activity(id);
                case NodeType.file:
                    return await Document(id);
                case NodeType.module:
                    return await Module(id);
                case NodeType.course:
                    return await Course(id);
                case NodeType.search:
                    return Search(id);
                case NodeType.root:
                    return await OnHome("root");

            }

            return new EmptyResult();
        }



        private async Task<IActionResult> Teacher(string id)
        {
            
            var teacher = await userManager.FindByIdAsync(id);

            if (teacher == null)
            {
                return new EmptyResult();
            }


            if (User.IsInRole("Teacher"))
            {
                var model = new TeacherFromTeacherModelView
                {
                    Id = id,
                    Name = $"{teacher.FirstName} {teacher.LastName}",
                    Email = teacher.Email

                };
                return PartialView("TeacherTeacher", model);

            }
            else if (User.IsInRole("Student"))
            {
                var model = new TeacherFromStudentModelView
                {
                    Id = id,
                    Name = $"{teacher.FirstName} {teacher.LastName}",
                    Email = teacher.Email

                };
                return PartialView("TeacherStudent", model);

            }

            return new EmptyResult();

        }

        private async Task<IActionResult> Student(string id)
        {
            var student=await userManager.FindByIdAsync(id);

            if (student == null)
            {
                return new EmptyResult();
            }
            
            if (User.IsInRole("Teacher"))
            {
                var model = new StudentFromTeacherModelView
                {
                    Id = id,
                    Name = $"{student.FirstName} {student.LastName}",
                    Email = student.Email
                
                };                        
                return PartialView("StudentTeacher", model);

            }
            else if (User.IsInRole("Student"))
            {
                var model = new StudentFromStudentModelView
                {
                    Id = id,
                    Name = $"{student.FirstName} {student.LastName}",
                    Email = student.Email

                };
                return PartialView("StudentStudent", model);

            }

            return new EmptyResult();
        }

        private async Task<IActionResult> Activity(string id)
        {
            int aid;
            if (!int.TryParse(id, out aid))
            {
                return new EmptyResult();
            }
            ActivityDto a = await uow.ActivityRepository.GetActivity(aid);

            if (a == null)
            {
                return new EmptyResult();
            }

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

        private async Task<IActionResult> Document(string id)
        {
            int did;
            if (!int.TryParse(id, out did))
            {
                return new EmptyResult();
            }
            var d = await uow.DocumentRepository.GetDocument(did);

            if (d == null)
            {
                return new EmptyResult();
            }

            var model = new DocumentModelView
            {
                Id = d.Id,
                Name=d.Name,
                Description=d.Description,
                DocumentUrl=d.DocumentUrl,
                TimeStamp=d.TimeStamp,
                PersonId=d.PersonId,
                PersonName=$"{d.PersonFirstName} {d.PersonLastName}"
            };

            return PartialView("Document", model);
        }

        private async Task<IActionResult> Course(string id)
        {
            int cid;
            if (!int.TryParse(id, out cid))
            {
                return new EmptyResult();
            }
            var c = await uow.CourseRepository.GetCourse(cid);

            if (c == null)
            {
                return new EmptyResult();
            }

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
            if (!int.TryParse(id, out mid))
            {
                return new EmptyResult();
            }
            var m = await uow.ModuleRepository.GetModule(mid);
            
            if (m == null)
            {
                return new EmptyResult();
            }

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
