using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAdmin.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            //Arrange
            var con = new HomeController();

            //Act
            var result = con.Index();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}