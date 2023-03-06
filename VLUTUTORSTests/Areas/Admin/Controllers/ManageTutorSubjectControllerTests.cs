using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VLUTUTORS.Models;
using Microsoft.EntityFrameworkCore;

namespace ManageTutorSubject.Tests
{
    [TestClass()]
    public class ManageTutorSubjectControllerTests
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageTutorSubjectController controller = new ManageTutorSubjectController();

            //Act
            ViewResult result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddSubjectTest_Get()
        {
            //Arrange
            ManageTutorSubjectController controller = new ManageTutorSubjectController();

            //Act
            ViewResult result = controller.AddSubject() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddSubjectTest_Post()
        {

        }

        [TestMethod()]
        public async Task EditSubjectTest_Get()
        {
            //Arrange
            ManageTestController controller = new ManageTestController();
            int id = 1;

            //Act
            var mon = await _context.Mongiasus.FirstOrDefaultAsync(m => m.IdmonGiaSu == id);

            // Assert
            Assert.IsNotNull(mon);
        }

        [TestMethod()]
        public void EditSubjectTest_Post()
        {

        }

        [TestMethod()]
        public void DeleteSubjectTest()
        {

        }
    }
}