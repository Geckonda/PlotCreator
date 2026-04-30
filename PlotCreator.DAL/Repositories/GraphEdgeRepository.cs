using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Repositories
{
    public class GraphEdgeRepository : IGraphEdgeRepository
    {
        private readonly ApplicationDBContext _db;

        public GraphEdgeRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(GraphEdge entity)
        {
            _db.GraphEdges.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(GraphEdge entity)
        {
            _db.GraphEdges.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<GraphEdge> Update(GraphEdge entity)
        {
            _db.GraphEdges.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public GraphEdge GetOne(int id) => _db.GraphEdges.First(e => e.Id == id);

        public IQueryable<GraphEdge> GetAll() => _db.GraphEdges;

        public async Task<GraphEdge?> FindAsync(int edgeId) =>
            await _db.GraphEdges.FirstOrDefaultAsync(e => e.Id == edgeId);
    }
}
