using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Areas.Admin.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ManageConsultingType.Tests
{
    [TestClass()]
    public class ManageConsultingTypeControllerTests
    {
        [TestMethod()]
        public async Task IndexTest()
        {

        }
        [TestMethod()]
        public void AddType_Get()
        {
            //Arrange
            ManageConsultingTypeController controller = new ManageConsultingTypeController();

            //Act
            ViewResult result = controller.AddType() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod()]
        public void AddType_Post()
        {
            //Arrange
            ManageConsultingTypeController controller = new ManageConsultingTypeController();

            //Act
            ViewResult result = controller.AddType() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void EditType()
        {
            //Arrange
            ManageConsultingTypeController controller = new ManageConsultingTypeController();

            //Act
            PartialViewResult result = controller.EditType(0) as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void EditType_Post()
        {
            //Arrange
            ManageConsultingTypeController controller = new ManageConsultingTypeController();

            //Act
            PartialViewResult result = controller.EditType(0) as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}