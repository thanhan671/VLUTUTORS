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

namespace ManageEvaluationCriteria.Tests
{
    [TestClass()]
    public class ManageEvaluationCriteriaControllerTests
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageEvaluationCriteriaController controller = new ManageEvaluationCriteriaController();

            //Act
            ViewResult result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public async Task EditCriteriaTest_Get()
        {
            //Arrange
            ManageEvaluationCriteriaController controller = new ManageEvaluationCriteriaController();
            int id = 1;
            //Act
            var checkTieuChi = await _context.Tieuchidanhgias.FirstOrDefaultAsync(m => m.IdTieuChi == id);

            // Assert
            Assert.IsNotNull(checkTieuChi);
        }

        [TestMethod()]
        public void EditCriteriaTest_Post()
        {

        }
    }
}