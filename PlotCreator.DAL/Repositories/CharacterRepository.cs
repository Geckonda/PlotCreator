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
			await DeleteEntitiesFromBook(entity.Books_Characters);
			await DeleteGroupsFromEntity(entity.Groups_Characters);
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
					.ThenInclude(x => x.Group)
				.First();
			character.Worldviews = _db.Worldview.ToList();

            return character;
        }

		public async Task<Character> GetEmptyViewModel(int bookId)
		{
			return new Character()
			{
				Worldviews = _db.Worldview.ToList(),
				Groups = _db.Groups
							.Where(x => x.BookId== bookId)
							.Where(x => x.Parent == "Character")
                            .GroupBy(x => x.Id)
                            .Select(x => x.FirstOrDefault()!)//! || ? 
							.ToList(),
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

		public async Task AddEntitiesToBook(IEnumerable<Book_Character> entity)
		{
			await _db.Books_Characters.AddRangeAsync(entity);
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

        public async Task DeleteEntitiesFromBook(IEnumerable<Book_Character> entities)
        {
            _db.Books_Characters.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }

		public IQueryable<Book_Character> GetBookEntitiesRelations(int bookId, int[] characterIds)
		{
			return _db.Books_Characters
						.Where(x => characterIds.Contains(x.CharacterId)
						&& x.BookId == bookId);
		}

		public async Task<Book> GetBook(int bookId)
		{
			return _db.Books
				.Where(x => x.Id == bookId)
				.Include(x => x.User)
				.Include(x => x.Access_Modificator)
				.Include(x => x.Rating)
				.Include(x => x.Genre)
				.Include(x => x.Book_Status)
				.First();
		}

        public async Task<IQueryable<Group_Character>> GetEntityGroups(int entityId)
        {
			return _db.Groups_Characters
					.Where(x => x.CharacterId == entityId);
        }

        public async Task<IQueryable<Group_Character>> GetAllEntityGroupsByBookId(int bookId)
        {
			return _db.Groups_Characters
					.Include(x => x.Group)
					.Where(x => x.Group.BookId == bookId);
        }

        public async Task AddGroupsToEntity(IEnumerable<Group_Character> groups)
        {
            await _db.Groups_Characters.AddRangeAsync(groups);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteGroupsFromEntity(IEnumerable<Group_Character> groups)
        {
			_db.RemoveRange(groups);
			await _db.SaveChangesAsync();
        }

        public async Task EditGroupsEntityRelation(IEnumerable<Group_Character> groups, int characterId, int bookId)
        {
            var groupsForDelete = _db.Groups_Characters
                                        .Where(x => x.CharacterId == characterId)
                                        .Include(x => x.Group)
                                        .Where(x => x.Group.BookId == bookId)
                                        .Where(x => !groups.Select(x => x.GroupId).Contains(x.GroupId))
                                        .ToList();

			if (groupsForDelete.Count > 0)
				await DeleteGroupsFromEntity(groupsForDelete);

			var existedGroups = _db.Groups_Characters
                                        .Where(x => x.CharacterId == characterId)
                                        .Include(x => x.Group)
                                        .Where(x => x.Group.BookId == bookId)
                                        .ToList();
			if(groups.Any()) 
				await AddGroupsToEntity(groups
					.Where(x => !existedGroups
					.Select(x => x.GroupId)
					.Contains(x.GroupId)));
        }
    }
}
