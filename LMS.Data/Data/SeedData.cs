using Bogus;
using LMS.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = LMS.Core.Models.Entities.Person;

namespace LMS.Data.Data
{
    public class SeedData
    {
        private static Faker fake;
        private static UserManager<User> userManager;
        private static RoleManager<IdentityRole> roleManager;

        public static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<ApplicationDbContext>())
            {
                fake = new Faker("sv");

                //FAKER FIRST

                //if (await db.Persons.AnyAsync()) return;

                //To Do: Add documents at activity level

                string teacherPw = "Hejsan123!";
                string studentPw = "Hoppsan123!";

                roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                if (roleManager is null) throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

                userManager = services.GetRequiredService<UserManager<User>>();
                if (userManager is null) throw new NullReferenceException(nameof(UserManager<User>));
   
                var roleNames = new[] { "Teacher", "Student" };

                await AddRolesAsync(roleNames);

                var teacher = await AddTeacherAsync(teacherPw);
                await AddTeacherToRoleAsync(teacher);

                var actTypeTypes = new List<string>
                {
                    "E-learning",
                    "Lecture",
                    "Exercise",
                    "Assignment",
                    "Group work"
                };

                var actTypes = GetActivityType(actTypeTypes);
                db.ActivityTypes.AddRange(actTypes);
                await db.SaveChangesAsync();

                var courseNames = new List<string> {
                    ".NET, programmerare och systemutvecklare",
                    "IT-supporttekniker",
                    "IT-tekniker Helpdesk 1st line support",
                    "System Developer Python, IT Security",
                    "Frontendutvecklare",
                    "Systemtestare",
                    "Informations- och IT-säkerhet"
                };

                var courses = GetCourses(courseNames, actTypes);
                db.Courses.AddRange(courses);

                var students = GetStudents(20, courses);
                await AddStudentsAsync(students, studentPw);
                await AddStudentsToRoleAsync(students);

                var documentsForCourses = GetDocumentsForCourses(students);
                db.Documents.AddRange(documentsForCourses);

                //await db.SaveChangesAsync();

                var documentsForModules = GetDocumentsForModules(students);
                db.Documents.AddRange(documentsForModules);

                await db.SaveChangesAsync();

                //var documentsForActivities = GetDocumentsForActivities(students);
                //db.Documents.AddRange(documentsForActivities);

                //await db.SaveChangesAsync();

            }
        }

        private static List<Document> GetDocumentsForModules(List<User> students)
        {
            var documents = new List<Document>();

            foreach (var student in students)
            {
                var modules = student.Course.Modules;

                foreach (var module in modules)
                {
                    var document = new Document
                    {
                        Name = fake.Company.CatchPhrase(),
                        Description = fake.Company.CompanySuffix() + fake.Random.Word(),
                        DocumentUrl = fake.Internet.UrlWithPath(),    //fake.Company.CatchPhrase(),
                        TimeStamp = DateTime.Now.AddDays(fake.Random.Int(-7, -2)),
                        Person = student,
                        Module = module
                    };
                    documents.Add(document);
                }
            }
            return documents;
        }

        private static List<Document> GetDocumentsForCourses(List<User> students)
        {
            var documents = new List<Document>();

            foreach (var student in students)
            {
                var document = new Document
                {
                    Name = fake.Company.CatchPhrase(),
                    Description = fake.Company.CompanySuffix() + fake.Random.Word(),
                    DocumentUrl = fake.Internet.UrlWithPath(),    //fake.Company.CatchPhrase(),
                    TimeStamp = DateTime.Now.AddDays(fake.Random.Int(-7, -2)),
                    Person = student,
                    Course = student.Course
                };
                documents.Add(document);
            }
            return documents;
        }

        private static List<Document> GetDocuments(int amount)
        {
            var documents = new List<Document>();
            for (int i = 0; i < amount; i++)
            {
                var document = new Document
                {
                    Name = fake.Company.CatchPhrase(),
                    Description = fake.Company.CompanySuffix() + fake.Random.Word(),
                    DocumentUrl = fake.Internet.UrlWithPath(),    //fake.Company.CatchPhrase(),
                    TimeStamp = DateTime.Now.AddDays(fake.Random.Int(-7, -2))
                };
                documents.Add(document);
            };
            return documents;

        }
        private static async Task AddStudentsAsync(List<User> students, string studentPw)
        {
            foreach (var student in students)
            {
                //var str = fake.Random.Replace("???###!").ToLower();
                //var studentPw = char.ToUpper(str[0]) + str.Substring(1);
                var result = await userManager.CreateAsync(student, studentPw);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<User> AddTeacherAsync(string teacherPw)
        {
            var firstName = fake.Name.FirstName();
            var lastName = fake.Name.LastName();
            var emailFirstName = firstName.ToLower();
            var emailLastName = lastName.ToLower();

            var teacher = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Documents = GetDocuments(2),
                Email = $"{emailFirstName}.{emailLastName}@lexicon.se",
                UserName = $"{emailFirstName}.{emailLastName}@lexicon.se"
            };

            var result = await userManager.CreateAsync(teacher, teacherPw);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return teacher;
        }

        private static List<ActivityType> GetActivityType(List<string> actTypeTypes)
        {
            var actTypes = new List<ActivityType>();

            foreach (var actType in actTypeTypes)
            {
                var activityType = new ActivityType
                {
                    Name = actType,
                    Description = fake.Lorem.Sentence()
                };
                actTypes.Add(activityType);
            }
            
            return actTypes;
        }

        private static List<Activity> GetActivities(List<ActivityType> actTypes)
        {
            var activities = new List<Activity>();

            foreach (var actType in actTypes)
            {
                var activity = new Activity
                {
                    Name = fake.Company.CompanySuffix() + fake.Random.Word(),
                    ActivityType = actType,
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-7, 12)),
                    EndDate = DateTime.Now.AddDays(fake.Random.Int(3, 20))
                };
                activities.Add(activity);
            }
            return activities;

        }

        private static List<Module> GetModules(int amount, List<ActivityType> actTypes)
        {
            var modules = new List<Module>();
            for (int i = 0; i < amount; i++)
            {
                var module = new Module
                {
                    Name = fake.Company.CompanySuffix() + fake.Random.Word(),
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-7, -2)),
                    EndDate = DateTime.Now.AddDays(fake.Random.Int(100, 150)),
                    Activities = GetActivities(actTypes)
                };
                modules.Add(module);
            }
            return modules;
        }

        private static List<Course> GetCourses(List<string> courseNames, List<ActivityType> actTypes)
        {
            var courses = new List<Course>();

            foreach (var courseName in courseNames)
            {
                var course = new Course
                {
                    Name = courseName,
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-7, -2)),
                    EndDate = DateTime.Now.AddDays(fake.Random.Int(90, 130)),
                    Modules = GetModules(3, actTypes)
            };
                courses.Add(course);
            }
            return courses;

        }
        private static List<User> GetStudents(int amount, List<Course> courses)
        {
            var students = new List<User>();

            for (int i = 0; i < amount; i++)
            {
                var firstName = fake.Name.FirstName();
                var lastName = fake.Name.LastName();
                var random = new Random();

                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Documents = GetDocuments(1),
                    UserName = fake.Internet.Email($"{firstName} {lastName}"),
                    Course = courses[random.Next(0, courses.Count)]
            };
                students.Add(user);
            }
            return students;

        }
        private static async Task AddStudentsToRoleAsync(List<User> students)
        {
            if (students is null) throw new NullReferenceException(nameof(students));

            foreach (var student in students)
            {
                if (await userManager.IsInRoleAsync(student, "Student")) continue;
                var result = await userManager.AddToRoleAsync(student, "Student");
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task AddTeacherToRoleAsync(User teacher)
        {
            if (teacher is null) throw new NullReferenceException(nameof(teacher));

            if (await userManager.IsInRoleAsync(teacher, "Teacher")) return;
            var result = await userManager.AddToRoleAsync(teacher, "Teacher");
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }
}