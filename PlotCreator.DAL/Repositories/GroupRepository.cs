using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDBContext _db;
        public GroupRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<IQueryable<Group>> GetAllByBookId(int bookId)
        {
            return _db.Groups
                .Where(x => x.BookId == bookId);
        }

        public Task<Book> GetBook(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}
