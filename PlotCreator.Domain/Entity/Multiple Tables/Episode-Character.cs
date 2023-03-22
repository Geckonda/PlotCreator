using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity.Multiple_Tables
{
    [Table("Episode-Character")]
    public class Episode_Character
    {
        public int Id { get; set; }

        public int EpisodeId { get; set; } // Вторичный ключ
        public Episode? Episode { get; set; } // Навигационное свойство


        public int CharacterId { get; set; } // Вторичный ключ
        public Character? Character { get; set; } // Навигационное свойство
    }
}
