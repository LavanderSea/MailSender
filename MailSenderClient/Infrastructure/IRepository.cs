using System.Collections.Generic;

namespace MailSenderClient.Infrastructure
{
    public interface IRepository<T>
    {
        void Set(T item);
        IEnumerable<T> GetAll();
    }
}
