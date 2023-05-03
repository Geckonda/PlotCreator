﻿using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class CharacterRepository : ICharacterRepository<Character>
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


        public async Task<IQueryable<Character>> GetAllCharacters(int userId)
        {
            return (from ch in _db.Characters
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
                    });
        }

        public async Task<IQueryable<Character>> GetBookCharacters(int bookId)
        {
            return (from ch in _db.Characters
                    where  ch.Books_Characters.FirstOrDefault().BookId == bookId
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

        public async Task<Character> GetOne(int id)
        {
            return (from ch in _db.Characters
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
                    }).First();
        }

       


        //Старые методы, который должны быть уничтожены
        public IQueryable<Character> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Character> GetAll(int parentId)
        {
            throw new NotImplementedException();
        }

		public async Task<CharacterViewModel> GetEmptyViewModel()
		{
            return new CharacterViewModel()
            {
                Worldviews = _db.Worldview.ToList(),
            };
		}
	}
}
