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

namespace ManageTest.Tests
{
    [TestClass()]
    public class ManageTestControllerTests
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageTestController controller = new ManageTestController();

            //Act
            ViewResult result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddQuestionTest_Get()
        {
            //Arrange
            ManageTestController controller = new ManageTestController();

            //Act
            ViewResult result = controller.AddQuestion() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddQuestionTest_Post()
        {

        }

        [TestMethod()]
        public async Task EditQuestionTest_Get()
        {
            //Arrange
            ManageTestController controller = new ManageTestController();
            int id = 1;

            //Act
            var baiKiemTra = await _context.Baikiemtras.FirstOrDefaultAsync(m => m.IdCauHoi == id);

            // Assert
            Assert.IsNotNull(baiKiemTra);
        }

        [TestMethod()]
        public void EditQuestionTest_Post()
        {

        }

        [TestMethod()]
        public void DeleteTest()
        {

        }
    }
}