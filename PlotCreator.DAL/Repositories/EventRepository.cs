using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class EventRepository : IPlotterRepository<Event>
    {
        private readonly ApplicationDBContext _db;

        public EventRepository(ApplicationDBContext db)
        {
            _db= db;
        }
        public async Task Add(Event entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Event entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Event> Update(Event entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Event> GetAll()
        {
            return _db.Events;
        }

        public async Task<IQueryable<Event>> GetAllByAnotherEntityId(int entityId)
        {
            return _db.Events
                .Where(x => x.BookId == entityId);
        }

        public Task<IQueryable<Event>> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetEmptyViewModel()
        {
            throw new NotImplementedException();
        }

        public async Task<Event> GetOne(int id)
        {
            return _db.Events
                .Where(x => x.Id == id)
                .First();
        }

    }
}
