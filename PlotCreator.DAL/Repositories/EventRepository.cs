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
	public class EventRepository : IEventRepository
	{
		private readonly ApplicationDBContext _db;

		public EventRepository(ApplicationDBContext db)
		{
			_db = db;
		}

		public async Task Add(Event entity)
		{
			await _db.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task AddCharactersToEntity(IEnumerable<Event_Character> characters)
		{
			await _db.Events_Characters.AddRangeAsync(characters);
			await _db.SaveChangesAsync();
		}

		public async Task AddEpisodesToEntity(IEnumerable<Episode_Event> episodes)
		{
			await _db.Episodes_Events.AddRangeAsync(episodes);
			await _db.SaveChangesAsync();
		}

		public async Task AddGroupsToEntity(IEnumerable<Group_Event> groups)
		{
			await _db.Groups_Events.AddRangeAsync(groups);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Event entity)
		{
			await DeleteEpisodesFromEntity(entity.Episodes_Events);
			await DeleteCharactersFromEntity(entity.Events_Characters);
			await DeleteGroupsFromEntity(entity.Groups_Events);
			_db.Remove(entity);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteCharactersFromEntity(IEnumerable<Event_Character> characters)
		{
			_db.Events_Characters.RemoveRange(characters);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteEpisodesFromEntity(IEnumerable<Episode_Event> episodes)
		{
			_db.Episodes_Events.RemoveRange(episodes);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteGroupsFromEntity(IEnumerable<Group_Event> groups)
		{
			_db.Groups_Events.RemoveRange(groups);
			await _db.SaveChangesAsync();
		}

		public async Task EditCharactersEntityRelation(IEnumerable<Event_Character> characters, int eventId, int bookId)
		{
			var charactersForDelete = _db.Events_Characters
										.Where(x => x.EventId == eventId)
										.Include(x => x.Character)
											.ThenInclude(x => x!.Books_Characters)
										.Where(x => x.Character!.Books_Characters.Select(x => x.BookId).Contains(bookId))
										.Where(x => characters.Select(x => x.CharacterId).Contains(x.CharacterId))
										.ToList();

			if (charactersForDelete.Count > 0)
				await DeleteCharactersFromEntity(charactersForDelete);
		}

		public async Task EditEpisodesEntityRelation(IEnumerable<Episode_Event> episodes, int eventId, int bookId)
		{
			var episodesForDelete = _db.Episodes_Events
										.Where(x => x.EventId == eventId)
										.Include(x => x.Episode)
										.Where(x => x.Episode!.BookId == bookId)
										.Where(x => episodes.Select(x => x.EpisodeId).Contains(x.EpisodeId))
										.ToList();

			if (episodesForDelete.Count > 0)
				await DeleteEpisodesFromEntity(episodesForDelete);
		}

		public async Task EditGroupsEntityRelation(IEnumerable<Group_Event> groups, int eventId, int bookId)
		{
			var groupsForDelete = _db.Groups_Events
										.Where(x => x.EventId == eventId)
										.Include(x => x.Group)
										.Where(x => x.Group!.BookId == bookId)
										.Where(x => groups.Select(x => x.GroupId).Contains(x.GroupId))
										.ToList();

			if (groupsForDelete.Count > 0)
				await DeleteGroupsFromEntity(groupsForDelete);
		}

		public IQueryable<Event> GetAll()
		{

			return _db.Events
				.Include(x => x.Book);
		}

		public async Task<IQueryable<Event>> GetAllByBookId(int bookId)
		{
			return _db.Events
				.Where(x => x.BookId == bookId)
				.Include(x => x.Book);
		}

		public async Task<IQueryable<Event>> GetAllByUserId(int userId)
		{
			return _db.Events
			.Include(x => x.Book)
			.Where(x => x.Book!.User!.Id == userId);
		}

		public async Task<IQueryable<Group_Event>> GetAllEntityGroupsByBookId(int bookId)
		{
			return _db.Groups_Events
					.Include(x => x.Group)
					.Where(x => x.Group!.BookId == bookId)
					.Where(x => x.Group!.Parent == "Event");
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

		public async Task<IQueryable<Event_Character>> GetEntityCharacters(int eventId)
		{
			return _db.Events_Characters
					.Where(x => x.EventId == eventId);
		}

		public async Task<IQueryable<Episode_Event>> GetEntityEpisodes(int eventId)
		{
			return _db.Episodes_Events
					.Where(x => x.EventId == eventId);
		}

		public async Task<IQueryable<Group_Event>> GetEntityGroups(int eventId)
		{
			return _db.Groups_Events
				.Where(x => x.EventId == eventId);
		}

		public Task<int> GetLastUserEntityId(int userId)
		{
			return _db.Events
				.Include(x => x.Book)
				.Where(x => x.Book!.User!.Id == userId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
				.FirstAsync();
		}

		public Event GetOne(int id)
		{
			return _db.Events
				.Include(x => x.Events_Characters)
					.ThenInclude(ev_ch => ev_ch.Character)
				.Include(x => x.Episodes_Events)
					.ThenInclude(ep_ev => ep_ev.Episode)
				.Include(x => x.Groups_Events)
					.ThenInclude(gr_ev => gr_ev.Group)
				.Where(x => x.Id == id)
				.First();
		}

		public async Task<EventLists> GetReferenceData(int? bookId)
		{
			return new EventLists()
			{
				Groups = _db.Groups
							.Where(x => x.BookId == bookId)
							.Where(x => x.Parent == "Event")
							.GroupBy(x => x.Id)
							.Select(x => x.FirstOrDefault()!)
							.ToList(),
				Episodes = _db.Episodes
							  .Where(x => x.BookId == bookId)
							  .GroupBy(x => x.Id)
							  .Select(x => x.FirstOrDefault()!)
							  .ToList(),
				Characters = _db.Characters
								.Include(x => x.Books_Characters)
									.ThenInclude(x => x.Book)
								.Where(x => x.Books_Characters.Select(x => x.BookId).Contains((int)bookId!))
								.GroupBy(x => x.Id)
								.Select(x => x.FirstOrDefault()!)
								.ToList(),
			};
		}

		public async Task<User> GetUser(int eventId)
		{
			return _db.Events
				.Where(e => eventId == e.Id)
				.Select(e => e.Book!.User!)
				.First();
		}

		public async Task<int> GetUserId(int eventId)
		{
			return _db.Events
				.Where(e => eventId == e.Id)
				.Select(e => e.Book!.User!.Id)
				.First();
		}

		public async Task<Event> Update(Event entity)
		{
			_db.Events.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
