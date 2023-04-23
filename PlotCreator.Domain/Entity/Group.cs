using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity
{
    public class Group
    {
        public int Id { get; set; }

        public int BookId { get; set; } // Вторичный ключ
        public Book? Book { get; set; } // Навигационное свойство

        public string? Name { get; set; }

        [Column(TypeName = "Ntext")]
        public string? Description { get; set; }


        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Group_Character> Groups_Characters { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Group_Event> Groups_Events { get; set; } = new();
    }
}
