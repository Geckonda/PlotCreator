using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class IdeaRepository : IBaseRepository<Idea>
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

        public async Task Delete(Idea entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Idea> GetAll()
        {
            return _db.Ideas;
        }

        public async Task<Idea> Update(Idea entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
