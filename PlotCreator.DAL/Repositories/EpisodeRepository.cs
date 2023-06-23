using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.ViewModels.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
	public class EpisodeRepository : IEpisodeRepository
	{
		private readonly ApplicationDBContext _db;
		public EpisodeRepository(ApplicationDBContext db)
		{
			_db = db;
		}
		public async Task Add(Episode entity)
		{
			await _db.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task AddCharactersToEntity(IEnumerable<Episode_Character> characters)
		{
			await _db.Episodes_Characters.AddRangeAsync(characters);
			await _db.SaveChangesAsync();
		}

		public async Task AddEventsToEntity(IEnumerable<Episode_Event> events)
		{
			await _db.Episodes_Events.AddRangeAsync(events);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Episode entity)
		{
			await DeleteEventsFromEntity(entity.Episodes_Events);
			await DeleteCharactersFromEntity(entity.Episodes_Characters);
			_db.Remove(entity);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteCharactersFromEntity(IEnumerable<Episode_Character> characters)
		{
			_db.Episodes_Characters.RemoveRange(characters);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteEventsFromEntity(IEnumerable<Episode_Event> events)
		{
			_db.Episodes_Events.RemoveRange(events);
			await _db.SaveChangesAsync();
		}

		public async Task EditCharactersEntityRelation(IEnumerable<Episode_Character> characters, int episodeId, int bookId)
		{
			var charactersForDelete = _db.Episodes_Characters
										.Where(x => x.EpisodeId == episodeId)
										.Include(x => x.Character)
											.ThenInclude(x => x!.Books_Characters)
										.Where(x => x.Character!.Books_Characters.Select(x => x.BookId).Contains(bookId))
										.Where(x => characters.Select(x => x.CharacterId).Contains(x.CharacterId))
										.ToList();

			if (charactersForDelete.Count > 0)
				await DeleteCharactersFromEntity(charactersForDelete);
		}

		public async Task EditEventsEntityRelation(IEnumerable<Episode_Event> events, int episodeId, int bookId)
		{
			var eventsForDelete = _db.Episodes_Events
										.Where(x => x.EpisodeId == episodeId)
										.Include(x => x.Event)
										.Where(x => x.Event!.BookId == bookId)
										.Where(x => events.Select(x => x.EventId).Contains(x.EventId))
										.ToList();

			if (eventsForDelete.Count > 0)
				await DeleteEventsFromEntity(eventsForDelete);
		}

		public IQueryable<Episode> GetAll()
		{
			return _db.Episodes
				.Include(x => x.Book);
		}

		public async Task<IQueryable<Episode>> GetAllByBookId(int bookId)
		{
			return _db.Episodes
				.Where(x => x.BookId == bookId)
				.Include(x => x.Book);
		}

		public async Task<IQueryable<Episode>> GetAllByUserId(int userId)
		{
			return _db.Episodes
				.Include(x => x.Book)
				.Include(x => x.Book!.User)
				.Where(x => x.Book!.User!.Id == userId);
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

		public async Task<IQueryable<Episode_Character>> GetEntityCharacters(int episodeId)
		{
			return _db.Episodes_Characters
					.Where(x => x.EpisodeId == episodeId);
		}

		public async Task<IQueryable<Episode_Event>> GetEntityEvents(int episodeId)
		{
			return _db.Episodes_Events
					.Where(x => x.EpisodeId == episodeId);
		}

		public Task<int> GetLastUserEntityId(int userId)
		{
			return _db.Episodes
                .Include(x => x.Book)
                .Where(x => x.Book!.User!.Id == userId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
				.FirstAsync();
		}

		public Episode GetOne(int id)
		{
			return _db.Episodes
				.Include(episode => episode.Episodes_Events)
					.ThenInclude(ep_ev => ep_ev.Event)
				.Include(episode => episode.Episodes_Characters)
					.ThenInclude(ep_ch => ep_ch.Character)
				.Where(episode => episode.Id == id)
				.First();
		}

        public async Task<EpisodeLists> GetReferenceData(int? bookId)
        {
			return new EpisodeLists()
			{
				Characters = _db.Characters
								.Include(x => x.Books_Characters)
									.ThenInclude(x => x.Book)
                                .Where(x => x.Books_Characters.Select(x => x.BookId).Contains((int)bookId!))
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

        public async Task<User> GetUser(int episodeId)
		{
			return _db.Episodes
				.Where(episode => episodeId == episode.Id)
				.Select(episode => episode.Book!.User!)
				.First();
		}

		public async Task<int> GetUserId(int episodeId)
		{
			return _db.Episodes
				.Where(episode => episodeId == episode.Id)
				.Select(episode => episode.Book!.User!.Id)
				.First();
		}

		public async Task<Episode> Update(Episode entity)
		{
			_db.Episodes.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
