using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Helpers.SQL;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationDBContext _db;
        public CharacterRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(Character entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Character entity)
        {
			await DeleteCharactersFromBook(entity.Books_Characters);
			//Удаление из эпизодов
			//Удаление из событий
			_db.Characters.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<Character> Update(Character entity)
        {
            _db.Characters.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Character> GetOne(int id)
        {
			var character = _db.Characters
				.Where(x => x.Id == id)
				.Include(x => x.Worldview)
				.Include(x => x.Books_Characters)
				.Include(x => x.Events_Characters)
				.Include(x => x.Groups_Characters)
				.First();
			character.Worldviews = _db.Worldview.ToList();

            return character;
        }

		public async Task<Character> GetEmptyViewModel()
		{
			return new Character()
			{
				Worldviews = _db.Worldview.ToList(),
			};
		}
		public async Task<IQueryable<Character>> GetAllByUserId(int userId)
		{
            return _db.Characters
				.Where(x => x.UserId == userId)
				.Include(x => x.Worldview);
		}

		public async Task<IQueryable<Character>> GetAllByBookId(int bookId)
		{
			return _db.Books_Characters
						.Include(x => x.Character)
							.ThenInclude(x => x.Worldview)
						.Where(x => x.BookId == bookId)
						.Select(x => x.Character!);
		}

		public IQueryable<Character> GetAll()
		{
			return _db.Characters;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId">ID пользователя</param>
		/// <param name="bookId">ID книги</param>
		/// <returns>Возвращает всех персонажей, незакрепленных текущей книгой. (Без дубликатов)</returns>
		public Task<IQueryable<Character>> GetAllExcludeCurrentBookCharacters(int userId, int bookId)
		{
			//ID Персонажей ПОЛЬЗОВАТЕЛЯ, неприкрепленные ни к одной из книг
            var CharactersWithNullBookId = _db.Characters
							.Include(x => x.Books_Characters)
							.Where(x =>  x.UserId == userId)
							.Where(x => x.Books_Characters.Count == 0)
                            .Select(x => x.Id);

			//ID персонажей текущей книги
			var CharactersFromCurrentBook = _db.Books_Characters
							.Where(x => x.BookId == bookId)
                            .Select(x => x.CharacterId);

			//ID персонажей, принадлежащих пользователю, но не принадлежащих текущей книге
			var CharactersFromOthersUserBooks = _db.Books_Characters
                            .Include(x => x.Character)
							.Where(x => x.Character!.UserId == userId)
							.Where(x => x.BookId != bookId)
							.Select(x => x.CharacterId);

			var characters = _db.Characters
				.Where(x =>
					(!CharactersFromCurrentBook.Contains(x.Id)
					&& CharactersFromOthersUserBooks.Contains(x.Id))
				|| CharactersWithNullBookId.Contains(x.Id))
				.Include(x => x.Worldview)
				.Include(x => x.Books_Characters)
				.Where(x => x.UserId == userId);

			return Task.FromResult(characters);
		}

		public async Task AddCharactersToBook(Book_Character entity)
		{
			await _db.Books_Characters.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

        public async Task<int> GetLastUserCharacterId(int userId)
        {
			return _db.Characters
				.Where(x => x.UserId == userId)
				.OrderByDescending(x => x.Id)
                .Select(x => x.Id)
				.First();
        }

        public async Task DeleteCharactersFromBook(IEnumerable<Book_Character> entities)
        {
            _db.Books_Characters.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }

		public IQueryable<Book_Character> GetBookCharactersRelations(int bookId, int[] characterIds)
		{
			return _db.Books_Characters
						.Where(x => characterIds.Contains(x.CharacterId)
						&& x.BookId == bookId);
		}
	}
}
