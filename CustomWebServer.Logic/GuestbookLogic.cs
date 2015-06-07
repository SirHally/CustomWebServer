using System.Collections.Generic;
using System.Linq;
using CustomWebServer.DAL.POCO;
using CustomWebServer.DAL.Repository;
using CustomWebServer.DAL.Specifications.POCO.User;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Types;

namespace CustomWebServer.Logic
{
    /// <summary>
    /// Логика управления сообщениями
    /// </summary>
    public class GuestbookLogic
    {
        /// <summary>
        /// Получаем все сообщения
        /// </summary>
        /// <returns></returns>
        public virtual IReadOnlyCollection<MessageResult> GetAll()
        {
            using (var context = IoCContainer.Get<IEntity>())
            {
                var repository = context.GetRepository<IRepository<Message>>();

                IEnumerable<Message> result = repository.Find(null);

                return result.Select(t => new MessageResult()
                {
                    User = t.User.Name,
                    Text = t.Text
                }).ToList();
            }
        }

        /// <summary>
        /// Создать новое сообщение
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="text"></param>
        public virtual void CreateMessage(string userName, string text)
        {
            using (var context = IoCContainer.Get<IEntity>())
            {
                var userRepository = context.GetRepository<IRepository<User>>();

                User user =
                    userRepository.Find(new ByName(userName)).SingleOrDefault();

                if (user == null)
                {
                    user = new User()
                    {
                        Name = userName
                    };
                    userRepository.Add(user);
                }

                var messageRepository = context.GetRepository<IRepository<Message>>();

                messageRepository.Add(new Message()
                {
                    Text = text,
                    User = user
                });

                context.SaveChanges();
            }
        }
    }
}
