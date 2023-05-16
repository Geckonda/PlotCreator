using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly ApplicationDBContext _db;
        

        public IdeaRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(Idea entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task AddIdeasToBook(IEnumerable<Book_Idea> entities)
        {
            await _db.AddRangeAsync(entities);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Idea entity)
		{
            await DeleteIdeasFromBook(entity.Books_Ideas);

			_db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteIdeasFromBook(IEnumerable<Book_Idea> entities)
        {
            _db.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Idea> GetAll()
        {
            return _db.Ideas;
        }

        public async Task<IQueryable<Idea>> GetAllByBookId(int bookId)
        {
            return _db.Books_Ideas
                        .Include(x => x.Idea)
                        .Where(x => x.BookId == bookId)
                        .Select(x => x.Idea!);
        }

        public async Task<IQueryable<Idea>> GetAllByUserId(int userId)
        {
            return _db.Ideas
                .Where(x => x.UserId == userId);
        }

        public async Task<IQueryable<Idea>> GetAllExcludeCurrentBookIdeas(int userid, int bookId)
        {
            var ideasWithNullBookId = _db.Ideas
                                        .Include(x => x.Books_Ideas)
                                        .Where(x => x.UserId == userid)
                                        .Where(x => x.Books_Ideas.Count == 0)
                                        .Select(x => x.Id);

            var ideasFromCurrentBook = _db.Books_Ideas
                                        .Where(x => x.BookId == bookId)
                                        .Select(x => x.IdeaId);

            var ideasFromOthersUserBooks = _db.Books_Ideas
                                        .Include(x => x.Idea)
                                        .Where(x => x.Idea!.UserId == userid)
                                        .Where(x => x.BookId != bookId)
                                        .Select(x => x.IdeaId);

            return _db.Ideas
                .Where(x =>
                    (!ideasFromCurrentBook.Contains(x.Id)
                    && ideasFromOthersUserBooks.Contains(x.Id))
                || ideasWithNullBookId.Contains(x.Id))
                .Include(x => x.Books_Ideas)
                .Where(x => x.UserId == userid);
        }

        public IQueryable<Book_Idea> GetBookIdeasRelations(int bookId, int[] ideaIds)
        {
            return _db.Books_Ideas
                .Where(x => ideaIds.Contains(x.IdeaId)
                && x.BookId == bookId);
        }

        public async Task<int> GetLastUserIdeaId(int userId)
        {
            return _db.Ideas
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .First();
        }

        public async Task<Idea> GetOne(int id)
        {
            return _db.Ideas
                .Where(x => x.Id == id)
                .Include(x => x.Books_Ideas)
                .First();
        }

        public async Task<Idea> Update(Idea entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
