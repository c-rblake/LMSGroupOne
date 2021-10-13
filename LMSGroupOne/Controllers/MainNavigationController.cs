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
        public MainNavigationController()
        { 
        
        }

        public IActionResult Index()
        {
            var model = new TreeNode
            {
                Id = "1",
                Name = "LMS",
                Type = NodeType.root,
                CanCreate=NodeType.none,
                Editable = true,                
                Nodes = new TreeNode[]
                {
                    MakeCourseBranch(),
                    MakeTeacherBranch(),
                    new TreeNode
                    {
                        Id="1",
                        Name="Literature Search",
                        Type=NodeType.search,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    }
                }                
            };

            return View(model);

           
        }

        private TreeNode MakeTeacherBranch()
        {
            TreeNode model = new TreeNode
            {
                Id = "202",
                Name = "Teachers",
                Type = NodeType.folder,
                CanCreate = NodeType.teacher,
                Editable = true,
                Open=true,
                Nodes = new TreeNode[]
                {
                    new TreeNode
                    {
                        Id="1",
                        Name="Björnbuse1",
                        Type=NodeType.teacher,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="2",
                        Name="Björnbuse2",
                        Type=NodeType.teacher,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="3",
                        Name="Björnbuse3",
                        Type=NodeType.teacher,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    }

                }                
            };
            return model;
        }

        private TreeNode MakeStudentBranch()
        {
            TreeNode model = new TreeNode
            {
                Id="101",
                Name="Students",
                Type=NodeType.folder,
                CanCreate = NodeType.student,
                Editable = true,
                Nodes =new TreeNode[] 
                { 
                    new TreeNode
                    { 
                        Id="1",
                        Name="Kalle Anka",
                        Type=NodeType.student,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="2",
                        Name="Kajsa Anka",
                        Type=NodeType.student,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="3",
                        Name="Joakim von Anka",
                        Type=NodeType.student,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="4",
                        Name="Musse Pig",
                        Type=NodeType.student,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="5",
                        Name="Magica de Hex",
                        Type=NodeType.student,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    }
                }
            };

            return model;
        }
        private TreeNode MakeCourseBranch()
        {
            TreeNode model = new TreeNode
            {
                Id = "51",
                Name = "Courses",
                Type = NodeType.folder,
                CanCreate = NodeType.course,
                Editable = true,
                Nodes = new TreeNode[]
                {
                    new TreeNode
                    {
                        Id="52",
                        Name="Course1",
                        Type=NodeType.course,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeModuleBranch(),
                            MakeStudentBranch(),
                            MakeFileBranch()                            
                        }
                    },
                    new TreeNode
                    {
                        Id="53",
                        Name="Course2",
                        Type=NodeType.course,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeModuleBranch(),
                            MakeStudentBranch(),
                            MakeFileBranch()                            
                        }
                    },
                    new TreeNode
                    {
                        Id="54",
                        Name="Course3",
                        Type=NodeType.course,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeModuleBranch(),
                            MakeStudentBranch(),
                            MakeFileBranch()                            
                        }
                    }

                }
            };


            return model;
        }

        private TreeNode MakeModuleBranch()
        {
            TreeNode model = new TreeNode
            {
                Id = "41",
                Name = "modules",
                Type = NodeType.folder,
                CanCreate = NodeType.module,
                Editable = true,
                Nodes = new TreeNode[]
                {
                    new TreeNode
                    {
                        Id="42",
                        Name="module1",
                        Type=NodeType.module,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeActivityBranch(),
                            MakeFileBranch()
                            
                        }
                    },
                    new TreeNode
                    {
                        Id="43",
                        Name="module2",
                        Type=NodeType.module,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeActivityBranch(),
                            MakeFileBranch()
                            
                        }
                    },
                    new TreeNode
                    {
                        Id="44",
                        Name="module3",
                        Type=NodeType.module,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeActivityBranch(),
                            MakeFileBranch()                            
                        }
                    }
                }
            };

            return model;
        }

        private TreeNode MakeActivityBranch()
        {
            TreeNode model = new TreeNode
            {
                Id = "11",
                Name = "Activities",
                Type = NodeType.folder,
                CanCreate = NodeType.activity,
                Editable = true,
                Nodes = new TreeNode[]
                { 
                    new TreeNode
                    { 
                        Id="13",
                        Name="Activity1",
                        Type=NodeType.activity,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeFileBranch()
                        }
                    },
                    new TreeNode
                    {
                        Id="14",
                        Name="Activity2",
                        Type=NodeType.activity,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeFileBranch()
                        }
                    },
                    new TreeNode
                    {
                        Id="15",
                        Name="Activity3",
                        Type=NodeType.activity,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=new TreeNode[]
                        {
                            MakeFileBranch()
                        }
                    }

                }
            };

            return model;
        }

        private TreeNode MakeFileBranch()
        {
            
            TreeNode model=new TreeNode
            {
                Id="98",
                Name="Documents",
                Type=NodeType.folder,
                CanCreate = NodeType.file,
                Editable = true,
                Nodes =new TreeNode[]
                {
                    new TreeNode
                    {
                        Id="31",
                        Name="Index.cshtml",
                        Type=NodeType.file,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="32",
                        Name="instructions.txt",
                        Type=NodeType.file,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    },
                    new TreeNode
                    {
                        Id="33",
                        Name="flowchart.png",
                        Type=NodeType.file,
                        CanCreate=NodeType.none,
                        Editable = true,
                        Nodes=null
                    }
                }               
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

        

        public IActionResult OnTreeClick(string id, string type)
        {
            Debug.WriteLine($"from controller- id:{id}, type:{type}");

            Debug.WriteLine("---------------------------------------------------");
            switch (type)
            {
                case "teacher":
                    return Teacher(id);
                case "student":
                    return Student(id);
                case "activity":
                    return Activity(id);
                case "file":
                    return Document(id);
                case "module":
                    return Module(id);
                case "course":
                    return Course(id);
                case "search":
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

        private IActionResult Activity(string id)
        {
            var model = new PlaceholderModelView
            {
                Id = id
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

        private IActionResult Course(string id)
        {
            var model = new PlaceholderModelView
            {
                Id = id
            };

            return PartialView("Course", model);
        }

        private IActionResult Module(string id)
        {
            var model = new PlaceholderModelView
            {
                Id = id
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
