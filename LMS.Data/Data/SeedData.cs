﻿using Bogus;
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



                if (await db.Persons.AnyAsync()) return;

                const string roleName = "Student";

                var userManager = services.GetRequiredService<UserManager<User>>();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var role = new IdentityRole { Name = roleName };
                var addRoleResult = await roleManager.CreateAsync(role);

                var courseNames = new List<string> { "Net21Best","Net21Average"};
                var courses = GetCourses(courseNames); // Course => Collection<modules>
                var modules = db.Modules.ToList();
                
                db.Courses.AddRange(courses);
                await db.SaveChangesAsync();


                var persons = GetPerson();
                db.Persons.AddRange(persons);
                //await db.SaveChangesAsync();

                //var modules = GetModules(); //Add from course
                //db.Modules.AddRange(modules);

                var activitityTypes = GetActivityType();
                db.ActivityTypes.AddRange(activitityTypes);
                await db.SaveChangesAsync();


                var activities = GetActivities(activitityTypes, modules);
                db.Activities.AddRange(activities);

                await db.SaveChangesAsync();





            }

        }

        private static List<ActivityType> GetActivityType()
        {
            var activityTypes = new List<ActivityType>();
            for (int i = 0; i < 5; i++)
            {
                var activityType = new ActivityType
                {
                    Name = fake.Company.CompanySuffix() + fake.Random.Word(),
                    Description = fake.Lorem.Sentence()
                };
                activityTypes.Add(activityType);
            }
            return activityTypes;
        }

        private static List<Activity> GetActivities(List<ActivityType> activityTypes, List<Module> modules)
        {
            var activities = new List<Activity>();
            for (int i = 0; i < 2; i++)
            {
                var activity = new Activity
                {
                    Name = fake.Company.CompanySuffix() + fake.Random.Word(),
                    ActivityTypeId = fake.Random.Int(1, activityTypes.Count),
                    ModuleId = fake.Random.Int(1, modules.Count),
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-7, 12)),
                    EndDate = DateTime.Now.AddDays(fake.Random.Int(100, 150))                    
                };
                activities.Add(activity); // 12421
            }
            return activities;

        }

        private static List<Module> GetModules()
        {
            var modules = new List<Module>();
            for (int i = 0; i < 5; i++)
            {
                var module = new Module
                {
                    CourseId = 1, //Todo
                    Name = fake.Company.CompanySuffix() + fake.Random.Word(),
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-7, -2)),
                    EndDate = DateTime.Now.AddDays(fake.Random.Int(100, 150))
                };
                modules.Add(module);
            }
            return modules;
        }

        private static List<Course> GetCourses(List<string> courseNames)
        {
            var courses = new List<Course>();

            foreach (var courseName in courseNames)
            {
                var course = new Course
                {
                    Name = courseName,
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-7, -2)),
                    EndDate = DateTime.Now.AddDays(fake.Random.Int(100, 150)),
                    Modules = GetModules()
                };
                courses.Add(course);
            }
            return courses;

        }

        private static List<Document> GetDocuments()
        {
            var documents = new List<Document>();
            for (int i = 0; i < 2; i++)
            {
                var document = new Document
                {
                    Name = fake.Company.CatchPhrase(),
                    Description = fake.Company.CompanySuffix() + fake.Random.Word(),
                    DocumentUrl = fake.Company.CatchPhrase(),
                    TimeStamp = DateTime.Now.AddDays(fake.Random.Int(-7, -2))
                };
                documents.Add(document);
            };
            return documents;

        }

        private static List<User> GetPerson()
        {
            var users = new List<User>();

            for (int i = 0; i < 20; i++)
            {
                var user = new User
                {
                    FirstName = fake.Name.FirstName(),
                    LastName = fake.Name.LastName(),
                    Documents = GetDocuments()
                };
                users.Add(user);
            }

            return users;

        }
    }
}