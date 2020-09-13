using System.Collections.Generic;

namespace MailSenderClient.Infrastructure
{
    public interface IRepository<T>
    {
        /// <summary>
        ///     Insert an item into database
        /// </summary>
        void Insert(T item);

        /// <summary>
        ///     Get all items from database
        /// </summary>
        IEnumerable<T> GetAll();
    }
}