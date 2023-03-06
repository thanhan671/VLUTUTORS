using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Areas.Admin.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ManageConsultingType.Tests
{
    [TestClass()]
    public class ManageConsultingTypeControllerTests
    {
        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            ManageConsultingTypeController controller = new ManageConsultingTypeController();

            //Act
            ViewResult result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
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
        [TestMethod]
        public async Task EditType()
        {
            //Arrange
            ManageConsultingTypeController controller = new ManageConsultingTypeController();

            //Act
            ViewResult result = await controller.EditType(0) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}