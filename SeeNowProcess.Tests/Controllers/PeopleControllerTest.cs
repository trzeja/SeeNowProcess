using System;
using NUnit.Framework;
using SeeNowProcess.Models;
using System.Web.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SeeNowProcess.Controllers;
using System.Web.Mvc;
using SeeNowProcess.Tests.DAL;
using SeeNowProcess.Models.Enums;
using System.Collections;

namespace SeeNowProcess.Tests.Controllers
{
    [TestFixture]
    public class PeopleControllerTest
    {
        [Test]
        public void Index_SholuldReturnAllPeople()
        {
            var context = new TestSeeNowContext();
            context.Users.Add(new User
            {
                Login = "asdfg",
                Email = "As.Dfg@company.com",
                Name = "As Dfg",
                Password = "admin11",
                PhoneNumber = "111-222-333",
                role = Role.Admin
            });
            context.Users.Add(new User
            {
                Login = "qwert",
                Email = "Qw.Ert@company.com",
                Name = "Qw Ert",
                Password = "secret1",
                PhoneNumber = "123-456-789",
                role = Role.HeadMaster
            });
            context.Users.Add(new User
            {
                Login = "kajak",
                Email = "Kaj.Kajak@company.com",
                Name = "Kaj Kajak",
                Password = "parostatkiem",
                PhoneNumber = "555-666-777",
                role = Role.SeniorDev
            });

            var controller = new PeopleController(context);
            var result = controller.Index(null) as ViewResult;
            var users = (List<User>)result.ViewData.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, users.Count);
        }
    }
}