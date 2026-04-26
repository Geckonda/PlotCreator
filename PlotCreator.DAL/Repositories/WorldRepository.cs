using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Repositories
{
    public class WorldRepository : IWorldRepository
    {
        private readonly ApplicationDBContext _db;

        public WorldRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(World entity)
        {
            _db.Worlds.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(World entity)
        {
            _db.Worlds.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<World> Update(World entity)
        {
            _db.Worlds.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public World GetOne(int id) => _db.Worlds.First(w => w.Id == id);

        public IQueryable<World> GetAll() => _db.Worlds;

        public async Task<IReadOnlyList<World>> GetByOwnerAsync(int ownerUserId) =>
            await _db.Worlds
                .Where(w => w.OwnerUserId == ownerUserId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<int> GetEntityCountAsync(int worldId)
        {
            var c = await _db.Characters.CountAsync(e => e.WorldId == worldId);
            var l = await _db.Locations.CountAsync(e => e.WorldId == worldId);
            var ev = await _db.Events.CountAsync(e => e.WorldId == worldId);
            var f = await _db.Factions.CountAsync(e => e.WorldId == worldId);
            var ep = await _db.Episodes.CountAsync(e => e.WorldId == worldId);
            var a = await _db.Artifacts.CountAsync(e => e.WorldId == worldId);
            var lo = await _db.Lores.CountAsync(e => e.WorldId == worldId);
            return c + l + ev + f + ep + a + lo;
        }
    }
}
