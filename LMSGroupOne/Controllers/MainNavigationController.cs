using LMSGroupOne.Models.MainNavigation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                CanCreate = NodeType.none,
                Editable = true,
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
                CanCreate = NodeType.none,
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
                CanCreate = NodeType.none,
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
                CanCreate = NodeType.none,
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
                CanCreate = NodeType.none,
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
                CanCreate = NodeType.none,
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
    }
}
