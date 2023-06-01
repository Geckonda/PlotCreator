using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System.ComponentModel.DataAnnotations;

namespace PlotCreator.Domain.Entity
{
    public class Event
    {
        public int Id { get; set; }

        public int BookId { get; set; } // Вторичный ключ
        public Book? Book { get; set; } // Навигационное свойство
		[Required]
		public string? Title { get; set; }

        [Column(TypeName = "NText")]
        public string? Description { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
        public bool ChekhovsGun { get; set; }
        public bool IsHidden { get; set; }
        public string? Icon { get; set; }
        public string? Colour { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Episode_Event> Episodes_Events { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Event_Character> Events_Characters { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Group_Event> Groups_Events { get; set; } = new();
    }
}
