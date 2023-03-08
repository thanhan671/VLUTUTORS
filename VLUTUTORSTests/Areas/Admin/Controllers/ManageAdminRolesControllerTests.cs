using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ManageAdminRoles.Tests
{
    [TestClass()]
    public class ManageAdminRolesControllerTests
    {
        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageAdminRolesController con = new ManageAdminRolesController();

            //Act
            ViewResult result = await con.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }
        [TestMethod()]
        public void AddRole_Get()
        {
            //Arrange
            ManageAdminRolesController con = new ManageAdminRolesController();

            //Act
            ViewResult result = con.AddRole() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task EditRole()
        {
            //Arrange
            ManageAdminRolesController con = new ManageAdminRolesController();
            int id = 0;
            //Act
            ViewResult result = await con.EditRole(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
