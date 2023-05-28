using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
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
            _db= db;
        }
        public async Task Add(Event entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Event entity)
        {
            await DeleteGroupsFromEntity(entity.Groups_Events);

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
            return _db.Events
                .Include(x => x.Book)
                .Include(x => x.Book!.User);
        }

        public async Task<IQueryable<Event>> GetAllByBookId(int bookId)
        {
            return _db.Events
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .Where(x => x.Book!.Id == bookId);
        }

        public async Task<IQueryable<Event>> GetAllByUserId(int userId)
        {
            return _db.Events
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .Where(x => x.Book!.User!.Id == userId);
        }

        public async Task<Event> GetEmptyViewModel(int bookId)
        {
            return new Event()
            {
                Groups = _db.Groups
                    .Where(x => x.BookId == bookId)
                    .Where(x => x.Parent == "Event")
                    .GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .ToList()
            };
        }

        public async Task<Event> GetOne(int id)
        {
            return _db.Events
                .Where(x => x.Id == id)
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .Include(x => x.Groups_Events)
                .First();
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

        public async Task<IQueryable<Group_Event>> GetEntityGroups(int entityId)
        {
            return _db.Groups_Events
                        .Where(x => x.EventId == entityId);
        }
        public async Task<int> GetLastUserEventId(int bookId)
        {
            return _db.Events
                .Where(x => x.BookId == bookId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .First();
        }

        public async Task<IQueryable<Group_Event>> GetAllEntityGroupsByBookId(int bookId)
        {
            return _db.Groups_Events
                    .Include(x => x.Group)
                    .Where(x => x.Group!.BookId == bookId);
        }

        public async Task AddGroupsToEntity(IEnumerable<Group_Event> groups)
        {
            await _db.Groups_Events.AddRangeAsync(groups);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteGroupsFromEntity(IEnumerable<Group_Event> groups)
        {
            _db.RemoveRange(groups);
            await _db.SaveChangesAsync();
        }

        public async Task EditGroupsEntityRelation(IEnumerable<Group_Event> groups, int eventId, int bookId)
        {
            var groupsForDelete = _db.Groups_Events
                                       .Where(x => x.EventId == eventId)
                                       .Include(x => x.Group)
                                       .Where(x => x.Group!.BookId == bookId)
                                       .Where(x => !groups.Select(x => x.GroupId).Contains(x.GroupId))
                                       .ToList();

            if (groupsForDelete.Count > 0)
                await DeleteGroupsFromEntity(groupsForDelete);

            var existedGroups = _db.Groups_Events
                                        .Where(x => x.EventId == eventId)
                                        .Include(x => x.Group)
                                        .Where(x => x.Group!.BookId == bookId)
                                        .ToList();
            if (groups.Any())
                await AddGroupsToEntity(groups
                    .Where(x => !existedGroups
                    .Select(x => x.GroupId)
                    .Contains(x.GroupId)));
        }
    }
}
