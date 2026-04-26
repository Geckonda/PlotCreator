using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.DAL.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            Stamp(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            Stamp(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void Stamp(DbContext? ctx)
        {
            if (ctx is null) return;

            var now = DateTime.UtcNow;

            foreach (var entry in ctx.ChangeTracker.Entries())
            {
                if (entry.Entity is EntityBase eb)
                {
                    if (entry.State == EntityState.Added)
                    {
                        eb.CreatedAt = now;
                        eb.UpdatedAt = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        eb.UpdatedAt = now;
                    }
                }
                else if (entry.Entity is World w)
                {
                    if (entry.State == EntityState.Added)
                    {
                        w.CreatedAt = now;
                        w.UpdatedAt = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        w.UpdatedAt = now;
                    }
                }
                else if (entry.Entity is Relation r && entry.State == EntityState.Added)
                {
                    r.CreatedAt = now;
                }
            }
        }
    }
}
