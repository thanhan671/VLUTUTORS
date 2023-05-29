using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VLUTUTORS.Requests.HistoryOfLearnings;

namespace History.Tests
{
    [TestClass()]
    public class HistoryOfLearningControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            //Arrange
            HistoryOfLearningController con = new HistoryOfLearningController();
            int id = 1;

            //Act
            ViewResult result = con.Index(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}