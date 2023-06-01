using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity
{
    public class Worldview
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Column(TypeName = "NText")]
        public string? Description { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v
        public List<Character> Characters { get; set; } = new();
    }
}
