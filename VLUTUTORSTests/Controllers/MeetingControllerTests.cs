using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VLUTUTORS.Requests.HistoryOfLearnings;

namespace Meeting.Tests
{
    [TestClass()]
    public class MeetingControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            //Arrange
            MeetingController con = new MeetingController();

            //Act
            ViewResult result = con.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}