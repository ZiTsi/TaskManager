using NHibernate;
using NHibernate.Util;
using TaskManager.Common;
using TaskManager.Common.Security;
using TaskManager.Data.Entities;
using TaskManager.Data.Exceptions;
using TaskManager.Data.QueryProcessors;

namespace TaskManager.Data.SqlServer.QueryProcessors
{
    public class AddTaskQueryProcessor : IAddTaskQueryProcessor
    {
        private readonly IDateTime _dateTime;
        private readonly ISession _session;
        private readonly IUserSession _userSession;

        public AddTaskQueryProcessor(ISession session, IUserSession userSession, IDateTime dateTime)
        {
            _session = session;
            _dateTime = dateTime;
            _userSession = userSession;
        }

        public void AddTask(Task task)
        {
            task.CreatedDate = _dateTime.UtcNow;
            task.Status = _session.QueryOver<Status>().Where(x => x.Name == "Not Started").SingleOrDefault();
            //task.CreatedBy = _session.QueryOver<User>().Where(x => x.Username == _userSession.Username).SingleOrDefault();
            task.CreatedBy = _session.Get<User>(1L);

            if (task.Users != null && task.Users.Any())
            {
                for (int i = 0; i < task.Users.Count; ++i)
                {
                    var user = task.Users[i];
                    var persistedUser = _session.Get<User>(user.UserId);
                    if (persistedUser == null)
                    {
                        throw new ChildObjectNotFoundException("User Not found");
                    }
                    task.Users[i] = persistedUser;
                }
            }
            _session.SaveOrUpdate(task);
        }
    }
}
