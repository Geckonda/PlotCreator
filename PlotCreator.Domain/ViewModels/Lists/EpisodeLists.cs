using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels.Lists
{
    public class EpisodeLists
    {
        public List<Character>? Characters { get; set; }
        public List<Event>? Events { get; set; }
    }
}
