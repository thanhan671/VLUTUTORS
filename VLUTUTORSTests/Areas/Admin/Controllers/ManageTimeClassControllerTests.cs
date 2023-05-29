using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLUTUTORS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ManageTimeClass.Tests
{
    [TestClass()]
    public class ManageTimeClassControllerTests
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageTimeClassController controller = new ManageTimeClassController();
            //Act
            ViewResult result = await controller.Index() as ViewResult;
            controller.HttpContext.Session.SetInt32("LoginADId", 1);
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddTimeClassTest_Get()
        {
            //Arrange
            ManageTimeClassController controller = new ManageTimeClassController();

            //Act
            ViewResult result = controller.AddTimeClass() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddTimeClassTest_Post()
        {

        }

        [TestMethod()]
        public async Task EditTimeClassTest_Get()
        {

        }

        [TestMethod()]
        public void EditTimeClassTest_Post()
        {

        }

        [TestMethod()]
        public void DeleteTimeClassTest()
        {

        }
    }
}