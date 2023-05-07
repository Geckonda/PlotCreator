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
				.First();
			character.Worldviews = _db.Worldview.ToList();

            return character;

           /* return (from ch in _db.Characters
                    where ch.Id == id
                    select new Character
                    {
                        Id = ch.Id,
                        UserId = ch.UserId,
                        Name = ch.Name,
                        Birthday = ch.Birthday,
                        Gender = ch.Gender,
                        Height = ch.Height,
                        Weight = ch.Weight,
                        Personality = ch.Personality,
                        Appearance = ch.Appearance,
                        Goals = ch.Goals,
                        Motivation = ch.Motivation, 
                        History = ch.History,
                        WorldviewId = ch.WorldviewId,
                        Worldview = ch.Worldview,
                        Worldviews = _db.Worldview.ToList(),
                        Picture = ch.Picture,
                        Deathday = ch.Deathday,
                    }).First();*/
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
			var characters = _db.Characters
				.Where(x => x.UserId == userId)
				.Include(x => x.Worldview);

            return characters;

            /*return (from ch in _db.Characters
					where ch.UserId == userId
					select new Character
					{
						Id = ch.Id,
						UserId = ch.UserId,
						Name = ch.Name,
						Birthday = ch.Birthday,
						Gender = ch.Gender,
						Height = ch.Height,
						Weight = ch.Weight,
						Personality = ch.Personality,
						Appearance = ch.Appearance,
						Goals = ch.Goals,
						Motivation = ch.Motivation,
						History = ch.History,
						WorldviewId = ch.WorldviewId,
						Worldview = ch.Worldview,
						Worldviews = _db.Worldview.ToList(),
						Picture = ch.Picture,
						Deathday = ch.Deathday,
					});*/
		}

		public async Task<IQueryable<Character>> GetAllByAnotherEntityId(int entityId)
		{
			return (from ch in _db.Characters
					join b_ch in _db.Books_Characters on ch.Id equals b_ch.CharacterId
					where b_ch.BookId == entityId
					select new Character
					{
						Id = ch.Id,
						UserId = ch.UserId,
						Name = ch.Name,
						Birthday = ch.Birthday,
						Gender = ch.Gender,
						Height = ch.Height,
						Weight = ch.Weight,
						Personality = ch.Personality,
						Appearance = ch.Appearance,
						Goals = ch.Goals,
						Motivation = ch.Motivation,
						History = ch.History,
						WorldviewId = ch.WorldviewId,
						Worldview = ch.Worldview,
						Worldviews = _db.Worldview.ToList(),
						Picture = ch.Picture,
						Deathday = ch.Deathday,
					});
		}

		public IQueryable<Character> GetAll()
		{
			return _db.Characters;
		}

		public async Task<IQueryable<Character>> GetAllExcludeCurrentBookCharacters(int userId, int bookId)
		{
			var SQLReaderHelper = new SQLCharacterReaderHelper(
				"Server=(localdb)\\MSSQLLocalDB;Database=Plot;Trusted_Connection=True;MultipleActiveResultSets=True",
				userId,
				bookId);
			int[] result = await SQLReaderHelper.ReadUnique();
			/*var characters = (from ch in _db.Characters
							 join b_ch in _db.Books_Characters on ch.Id equals b_ch.CharacterId
							 where (b_ch.BookId != bookId) && ch.UserId == userId
							 select new Character
							 {
								 Id = ch.Id,
								 UserId = ch.UserId,
								 Name = ch.Name,
								 Birthday = ch.Birthday,
								 Gender = ch.Gender,
								 Height = ch.Height,
								 Weight = ch.Weight,
								 Personality = ch.Personality,
								 Appearance = ch.Appearance,
								 Goals = ch.Goals,
								 Motivation = ch.Motivation,
								 History = ch.History,
								 WorldviewId = ch.WorldviewId,
								 Worldview = ch.Worldview,
								 Worldviews = _db.Worldview.ToList(),
								 Picture = ch.Picture,
								 Deathday = ch.Deathday,
							 });*/
			var characters = _db.Characters
				.Where(x => result.Contains(x.Id))
				.Include(x => x.Worldview)
				.Include(x => x.Books_Characters)
				.Where(x => x.UserId == userId);

			return characters;
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
    }
}
