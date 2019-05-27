using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja.Mocking.HouseKeeper;
using TestNinja.Mocking.HouseKeeper.TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    internal class HouseKeeperService_SendStatementEmailsTests {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private HousekeeperService _service;
        private Housekeeper _houseKeeper;

        private readonly DateTime _statementDate = new DateTime(2019, 05, 10);
        private string _statementFileName;

        [SetUp]
        public void SetUp() {
            _houseKeeper = new Housekeeper {
                Oid = 11,
                FullName = "Test name",
                Email = "testname@gmail.com",
                StatementEmailBody = "Hello"
            };

            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper> {
                _houseKeeper
            }.AsQueryable);

            _statementFileName = "fileName";

            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator
                .Setup(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
                .Returns(() => _statementFileName);

            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();

            _service = new HousekeeperService(_unitOfWork.Object, _statementGenerator.Object, _emailSender.Object, _messageBox.Object);


        }

        [Test]
        public void WhenCalled_GenerateStatements() {
            _service.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));
        }

        [Test]
        public void HouseKeeperEmailIsNullOrWhiteSpace_DontGenerateStatement([Values(null, "", " ")] string email) {
            _houseKeeper.Email = email;

            _service.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate), Times.Never);
        }

        [Test]
        public void WhenCalled_EmailTheStatement() {
            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                _houseKeeper.Email,
                _houseKeeper.StatementEmailBody,
                _statementFileName,
                It.IsAny<string>()));
        }

        [Test]
        public void StatementFileNameIsNull_DontEmailTheStatement([Values(null, "", " ")] string fileName) {
            _statementFileName = fileName;

            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void EmailSendingFail_ShowMessageBox() {
            _emailSender.Setup(es => es.EmailFile(
                _houseKeeper.Email,
                _houseKeeper.StatementEmailBody,
                _statementFileName,
                It.IsAny<string>()
            )).Throws<Exception>();

            _service.SendStatementEmails(_statementDate);

            _messageBox.Verify(mb => mb.Show(
                It.IsAny<string>(),
                It.IsAny<string>(),
                MessageBoxButtons.OK
            ));
        }
    }
}