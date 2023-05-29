using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLUTUTORS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookTutor.Tests
{
    [TestClass()]
    public class BookTutorControllerTests
    {
        [TestMethod()]
        public void HistoryBookingTest()
        {
            //Arrange
            BookTutorController con = new BookTutorController();
            int id = 1;

            //Act
            ViewResult result =  con.HistoryBooking(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}