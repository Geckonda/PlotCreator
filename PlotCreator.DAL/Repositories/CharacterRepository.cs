using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.ViewModels.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
	public class CharacterRepository: ICharacterRepository
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

		public async Task AddEntitiesToBook(IEnumerable<Book_Character> entities)
		{
			await _db.Books_Characters.AddRangeAsync(entities);
			await _db.SaveChangesAsync();
		}

		public async Task AddEpisodesToEntity(IEnumerable<Episode_Character> episodes)
		{
			await _db.Episodes_Characters.AddRangeAsync(episodes);
			await _db.SaveChangesAsync();
		}

		public async Task AddEventsToEntity(IEnumerable<Event_Character> events)
		{
			await _db.Events_Characters.AddRangeAsync(events);
			await _db.SaveChangesAsync();
		}

		public async Task AddGroupsToEntity(IEnumerable<Group_Character> groups)
		{
			await _db.Groups_Characters.AddRangeAsync(groups);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Character entity)
		{
			await DeleteEntitiesFromBook(entity.Books_Characters);
			await DeleteGroupsFromEntity(entity.Groups_Characters);
			await DeleteEpisodesFromEntity(entity.Episodes_Characters);
			await DeleteEventsFromEntity(entity.Events_Characters);
			_db.Characters.Remove(entity);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteEntitiesFromBook(IEnumerable<Book_Character> entities)
		{
			_db.Books_Characters.RemoveRange(entities);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteEpisodesFromEntity(IEnumerable<Episode_Character> episodes)
		{
			_db.Episodes_Characters.RemoveRange(episodes);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteEventsFromEntity(IEnumerable<Event_Character> events)
		{
			_db.Events_Characters.RemoveRange(events);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteGroupsFromEntity(IEnumerable<Group_Character> groups)
		{
			_db.Groups_Characters.RemoveRange(groups);
			await _db.SaveChangesAsync();
		}

		public async Task EditEpisodesEntityRelation(IEnumerable<Episode_Character> episodes, int characterId, int bookId)
		{
			var episodesForDelete = _db.Episodes_Characters
											.Where(x => x.CharacterId == characterId)
											.Include(x => x.Episode)
											.Where(x => x.Episode!.BookId == bookId)
											.Where(x => episodes.Select(x => x.EpisodeId).Contains(x.EpisodeId))
											.ToList();

			/*if(episodesForDelete.Count > 0)
				await DeleteEpisodesFromEntity(episodesForDelete);

			var existedEpisodes = _db.Episodes_Characters
										.Where(x => x.CharacterId == characterId)
										.Include(x => x.Episode)
										.Where(x => x.Episode!.BookId == bookId)
										.ToList();
			if (episodes.Any())
				await AddEpisodesToEntity(episodes
					.Where(x => !existedEpisodes
					.Select(x => x.EpisodeId)
					.Contains(x.EpisodeId)));*/
		}

		public async Task EditEventsEntityRelation(IEnumerable<Event_Character> events, int characterId, int bookId)
		{
			var eventsForDelete = _db.Events_Characters
										.Where(x => x.CharacterId == characterId)
										.Include(x => x.Event)
										.Where(x => x.Event!.BookId == bookId)
										.Where(x => events.Select(x => x.EventId).Contains(x.EventId))
										.ToList();

			if(eventsForDelete.Count > 0)
				await DeleteEventsFromEntity(eventsForDelete);

            /*var  existedEvents = _db.Events_Characters
										.Where(x => x.CharacterId == characterId)
										.Include(x => x.Event)
										.Where(x => x.Event!.BookId == bookId)
										.ToList();
			if (events.Any())
				await AddEventsToEntity(events
					.Where(x => !existedEvents
					.Select(x => x.EventId)
					.Contains(x.EventId)));*/
        }

        public async Task EditGroupsEntityRelation(IEnumerable<Group_Character> groups, int characterId, int bookId)
		{
			var groupsForDelete = _db.Groups_Characters
										.Where(x => x.CharacterId == characterId)
										.Include(x => x.Group)
										.Where(x => x.Group!.BookId == bookId)
										.Where(x => groups.Select(x => x.GroupId).Contains(x.GroupId))
										.ToList();

			if (groupsForDelete.Count > 0)
				await DeleteGroupsFromEntity(groupsForDelete);

			/*var existedGroups = _db.Groups_Characters
										.Where(x => x.CharacterId == characterId)
										.Include(x => x.Group)
										.Where(x => x.Group!.BookId == bookId)
										.ToList();
			if (groups.Any())
				await AddGroupsToEntity(groups
					.Where(x => !existedGroups
					.Select(x => x.GroupId)
					.Contains(x.GroupId)));*/
		}

		public IQueryable<Character> GetAll()
		{
			return _db.Characters;
		}

		public async Task<IQueryable<Character>> GetAllByBookId(int bookId)
		{
			return _db.Books_Characters
						.Include(x => x.Character)
						.Where(x => x.BookId == bookId)
						.Select(x => x.Character!);
		}

		public async Task<IQueryable<Character>> GetAllByUserId(int userId)
		{
			return _db.Characters
				.Where(x => x.UserId == userId);
		}

		public async Task<IQueryable<Group_Character>> GetAllEntityGroupsByBookId(int bookId)
		{
			return _db.Groups_Characters
					.Include(x => x.Group)
					.Where(x => x.Group!.BookId == bookId);
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
							.Where(x => x.UserId == userId)
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

		public IQueryable<Book_Character> GetBookEntitiesRelations(int bookId, int[] characterIds)
		{
			return _db.Books_Characters
						.Where(x => characterIds.Contains(x.CharacterId)
						&& x.BookId == bookId);
		}

		public async Task<IQueryable<Episode_Character>> GetEntityEpisodes(int characterId)
		{
			return _db.Episodes_Characters
				.Where(x => x.CharacterId == characterId);
		}

		public async Task<IQueryable<Event_Character>> GetEntityEvents(int characterId)
		{
			return _db.Events_Characters
				.Where(x => x.CharacterId == characterId);
		}

		public async Task<IQueryable<Group_Character>> GetEntityGroups(int characterId)
		{
			return _db.Groups_Characters
				.Where(x => x.CharacterId == characterId);
		}

		public async Task<int> GetLastUserEntityId(int userId)
		{
			return _db.Characters
				.Where(x => x.UserId == userId)
				.OrderByDescending(x => x.Id)
				.Select(x => x.Id)
				.First();
		}

		public Character GetOne(int id)
		{
			return _db.Characters
				.Include(character => character.Books_Characters)
					.ThenInclude(b_ch => b_ch.Book)
				.Include(character => character.Episodes_Characters)
					.ThenInclude(ep_ch => ep_ch.Episode)
				.Include(character => character.Events_Characters)
					.ThenInclude(ev_ch => ev_ch.Event)
				.Include(character => character.Groups_Characters)
					.ThenInclude(gr_ch => gr_ch.Group)
				.Where(character => character.Id == id)
				.First();
		}

		public async Task<CharacterLists> GetReferenceData(int? bookId)
		{
			return new CharacterLists()
			{
				Worldviews = _db.Worldview.ToList(),
				Groups = _db.Groups
							.Where(x => x.BookId == bookId)
							.Where(x => x.Parent == "Character")
							.GroupBy(x => x.Id)
							.Select(x => x.FirstOrDefault()!)
							.ToList(),
				Episodes = _db.Episodes
							  .Where(x => x.BookId == bookId)
							  .GroupBy(x => x.Id)
							  .Select(x => x.FirstOrDefault()!)
							  .ToList(),
				Events = _db.Events
							.Where(x => x.BookId == bookId)
							.GroupBy(x => x.Id)
							.Select(x => x.FirstOrDefault()!)
							.ToList(),
			};
		}

		public async Task<User> GetUser(int characterId)
		{
			return _db.Characters
				.Where(character => character.Id == characterId)
				.Select(character => character.User!)
				.First();
		}

		public async Task<int> GetUserId(int characterId)
		{
			return _db.Characters
				.Where(character => character.Id == characterId)
				.Select(character => character.User!.Id)
				.First();
		}

		public async Task<Character> Update(Character entity)
		{
			_db.Characters.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
