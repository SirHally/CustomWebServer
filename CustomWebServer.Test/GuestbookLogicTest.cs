using System;
using System.Collections.Generic;
using System.Linq;
using CustomWebServer.DAL.POCO;
using CustomWebServer.DAL.Repository;
using CustomWebServer.DAL.Specifications;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CustomWebServer.Test
{
    /// <summary>
    /// Тестируем GuestbookLogic
    /// </summary>
    [TestClass]
    public class GuestbookLogicTest
    {
        /// <summary>
        /// Тестируем получение всех сообщений
        /// </summary>
        [TestMethod]
        public void GuestbookLogic_GetAllTest()
        {
            var mockMessage = new Message()
            {
                Text = "Тестовое сообщение 1",
                User = new User()
                {
                    Name = "Пользователь"
                }
            };
            Mock<IRepository<Message>> mock = new Mock<IRepository<Message>>();
            mock.Setup(t => t.Find(null)).Returns(new List<Message>
            {
                mockMessage
            });
            IoCContainer.Rebind(typeof(IRepository<Message>), mock.Object);
            IoCContainer.Rebind(typeof(IEntity), new Mock<IEntity>().Object);

            var logic = new GuestbookLogic();
            var messages = logic.GetAll();

            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(mockMessage.Text, messages.Single().Text);
            Assert.AreEqual(mockMessage.User.Name, messages.Single().User);
        }

        /// <summary>
        /// Тестируем создание нового сообщения
        /// </summary>
        [TestMethod]
        public void GuestbookLogic_CreateMessageTest()
        {
            const string userName = "sirhally";
            const string text = "Test message";
            IoCContainer.Rebind(typeof(IEntity), new Mock<IEntity>().Object);

            var mockUser = new User()
            {
                Name = userName
            };
            Mock<IRepository<User>> mockUserRepository = new Mock<IRepository<User>>();
            mockUserRepository.Setup(t => t.Find(It.IsAny<ISpecification<User>>())).Returns(new List<User>
            {
                mockUser
            });
            IoCContainer.Rebind(typeof(IRepository<User>), mockUserRepository.Object);


            Mock<IRepository<Message>> mockMessageRepository = new Mock<IRepository<Message>>();
            mockMessageRepository.Setup(t => t.Add(It.IsAny<Message>()));
            IoCContainer.Rebind(typeof(IRepository<Message>), mockMessageRepository.Object);

            var logic = new GuestbookLogic();
            logic.CreateMessage(userName, text);

            mockMessageRepository.Verify(t => t.Add(It.Is<Message>(q => q.Text == text && q.User.Name == userName)), Times.Once);

            mockUserRepository.Verify(t => t.Find(It.Is<ISpecification<User>>(m => m.IsSatisfiedBy(mockUser))), Times.Once);
        }
    }
}
