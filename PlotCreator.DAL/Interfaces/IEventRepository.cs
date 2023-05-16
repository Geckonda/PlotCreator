using PlotCreator.DAL.Interfaces.Helpers;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEventRepository : IPlotterRepository<Event>, IBookHelper<Event>
    {

    }
}
