using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels
{
    public class IdeaViewModel
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public string? Topic { get; set; }
        public DateTime Data_Creation { get; set; }
        public string? Content { get; set; }

        public List<Book>? Books { get; set; } = new();
    }
}
