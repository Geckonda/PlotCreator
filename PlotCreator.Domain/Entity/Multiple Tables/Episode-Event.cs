using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity.Multiple_Tables
{
    [Table("Episode-Event")]
    public class Episode_Event
    {
        public int Id { get; set; }

        public int EpisodeId { get; set; } // Вторичный ключ
        public Episode? Episode { get; set; } // Навигационное свойство


        public int EventId { get; set; } // Вторичный ключ
        public Event? Event { get; set; } // Навигационное свойство
    }
}
