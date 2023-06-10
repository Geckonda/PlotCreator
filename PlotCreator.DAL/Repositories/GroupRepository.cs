using Microsoft.EntityFrameworkCore;
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
		public async Task Add(Group entity)
		{
			await _db.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Group entity)
		{
			_db.RemoveRange(entity.Groups_Characters);

			_db.Remove(entity);
			await _db.SaveChangesAsync();
		}

		public IQueryable<Group> GetAll()
		{
			return _db.Groups;
		}

		public async Task<IQueryable<Group>> GetAllByBookId(int bookId)
		{
			return _db.Groups
				.Where(x => x.BookId == bookId)
				.Include(x => x.Book)
					.ThenInclude(x => x!.User)
				.Include(x => x.Groups_Characters)
					.ThenInclude(x => x.Character)
				.Include(x => x.Groups_Events)
					.ThenInclude(x => x.Event);
		}


		public async Task<IQueryable<Group>> GetAllGroupsByParent(int bookId, string parent)
		{
			return _db.Groups
			   .Where(x => x.BookId == bookId)
			   .Where(x => x.Parent == parent)
			   .Include(x => x.Book)
					.ThenInclude(x => x!.User)
				.Include(x => x.Groups_Characters)
					.ThenInclude(x => x.Character)
				.Include(x => x.Groups_Events)
					.ThenInclude(x => x.Event);
		}

		public async Task<Book> GetBook(int bookId)
		{
			return _db.Books
				.Include(book => book.Books_Characters)
					.ThenInclude(b_ch => b_ch.Character)
				.Include(book => book.Books_Ideas)
					.ThenInclude(b_idea => b_idea.Idea)
				.Where(book => book.Id == bookId)
				.First();
		}


		public Group GetOne(int id)
		{
			return _db.Groups
						.Where(x => x.Id == id)
						.Include(x => x.Groups_Characters)
							.ThenInclude(x => x.Character)
						.Include(x => x.Groups_Events)
							.ThenInclude(x => x.Event)
						.First();
		}


		public async Task<Group> Update(Group entity)
		{
			_db.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
