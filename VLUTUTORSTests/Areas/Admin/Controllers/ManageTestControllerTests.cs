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
using Moq;
using System.Net.Sockets;
using System.Transactions;

namespace ManageTest.Tests
{
    [TestClass()]
    public class ManageTestControllerTests
    {
        private readonly CP25Team01Context _context = new CP25Team01Context();
        private IQueryable<Baikiemtra> _tests;
        private Mock<DbSet<Baikiemtra>> mockSet;
        private Mock<CP25Team01Context> _contextMock;
        //private UnitOfWork unitOfWork;
        private TransactionScope scope;

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
            _tests = new List<Baikiemtra> {
                new Baikiemtra() { CauHoi = "Ten cau hoi 1?", DapAnA = "A", DapAnB = "B", DapAnC = "C", DapAnD = "D", DapAnDung = "A" }
            }.AsQueryable();
            mockSet = new Mock<DbSet<Baikiemtra>>();
            _contextMock = new Mock<CP25Team01Context>();
            //unitOfWork = new UnitOfWork(_contextMock.Object);
            scope = new TransactionScope();
            mockSet.As<IQueryable<Baikiemtra>>().Setup(m => m.Provider).Returns(_tests.Provider);
            mockSet.As<IQueryable<Baikiemtra>>().Setup(m => m.Expression).Returns(_tests.Expression);
            mockSet.As<IQueryable<Baikiemtra>>().Setup(m => m.ElementType).Returns(_tests.ElementType);
            mockSet.As<IQueryable<Baikiemtra>>().Setup(m => m.GetEnumerator()).Returns(_tests.GetEnumerator());
            _contextMock.Setup(c => c.Baikiemtras).Returns(() => mockSet.Object);
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