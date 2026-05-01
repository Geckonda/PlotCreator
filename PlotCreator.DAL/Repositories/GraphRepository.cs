using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private readonly ApplicationDBContext _db;

        public GraphRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(Graph entity)
        {
            _db.Graphs.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Graph entity)
        {
            _db.Graphs.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Graph> Update(Graph entity)
        {
            _db.Graphs.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Graph GetOne(int id) => _db.Graphs.First(g => g.Id == id);

        public IQueryable<Graph> GetAll() => _db.Graphs;

        public async Task<IReadOnlyList<Graph>> GetByWorldAndUserAsync(int worldId, int userId) =>
            await _db.Graphs
                .Where(g => g.WorldId == worldId && g.OwnerUserId == userId)
                .OrderByDescending(g => g.IsSystemDefault)
                .ThenBy(g => g.Name)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Graph?> GetSummaryAsync(int graphId) =>
            await _db.Graphs
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == graphId);

        public async Task<Graph?> GetWithContentsAsync(int graphId) =>
            await _db.Graphs
                .Include(g => g.Nodes).ThenInclude(n => n.Entity!).ThenInclude(e => e.Type)
                .Include(g => g.Edges)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == graphId);

        public async Task<Graph> EnsureSystemDefaultAsync(int worldId, int userId)
        {
            var existing = await _db.Graphs
                .FirstOrDefaultAsync(g => g.WorldId == worldId
                                          && g.OwnerUserId == userId
                                          && g.IsSystemDefault);
            if (existing is not null) return existing;

            var def = new Graph
            {
                WorldId = worldId,
                OwnerUserId = userId,
                Name = "Smart graph",
                Description = "Auto-derived undirected graph (read-only).",
                IsSystemDefault = true,
                Kind = "SmartUndirected"
            };
            _db.Graphs.Add(def);
            await _db.SaveChangesAsync();
            return def;
        }
    }
}
