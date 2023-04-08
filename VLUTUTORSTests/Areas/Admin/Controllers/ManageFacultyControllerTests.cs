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

namespace ManageFaculty.Tests
{
    [TestClass()]
    public class ManageFacultyControllerTests
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();

        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageFacultyController controller = new ManageFacultyController();

            //Act
            ViewResult result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddFacultyTest_Get()
        {
            //Arrange
            ManageFacultyController controller = new ManageFacultyController();

            //Act
            ViewResult result = controller.AddFaculty() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddFacultyTest_Post()
        {

        }

        [TestMethod()]
        public async Task EditFacultyTest_Get()
        {
            //Arrange
            ManageEvaluationCriteriaController controller = new ManageEvaluationCriteriaController();
            int id = 1;

            //Act
            var checkKhoa = await _context.Khoas.FirstOrDefaultAsync(m => m.Idkhoa == id);

            // Assert
            Assert.IsNotNull(checkKhoa);
        }

        [TestMethod()]
        public void EditFacultyTest_Post()
        {

        }

        [TestMethod()]
        public void DeleteFacultyTest()
        {

        }
    }
}