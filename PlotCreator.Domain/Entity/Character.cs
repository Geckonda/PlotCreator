using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity.Multiple_Tables;

namespace PlotCreator.Domain.Entity
{
    public class Character
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Вторичный ключ
        public User? User { get; set; } // Навигационное свойство

        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        [Column(TypeName = "Text")]
        public string? Personality { get; set; }

        [Column(TypeName = "Text")]
        public string? Appearance { get; set; }

        [Column(TypeName = "Text")]
        public string? goals { get; set; }

        [Column(TypeName = "Text")]
        public string? motivation { get; set; }

        [Column(TypeName = "Text")]
        public string? History { get; set; }


        public int WorldviewId { get; set; } // Вторичный ключ
        public Worldview? Worldview { get; set; } // Навигационное свойство

        ////Файловая шняга
        //[NotMapped]
        //public File? Picture { get; set; } 
        public string? Picture { get; set; }
        public DateTime? Deathday { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Book_Character> Books_Characters { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Episode_Character> Episodes_Characters { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Event_Character> Events_Characters { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Group_Character> Groups_Characters { get; set; } = new();
    }
}
