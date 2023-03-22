using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity.Multiple_Tables
{
    [Table("Group-Character")]
    public class Group_Character
    {
        public int Id { get; set; }

        public int GroupId { get; set; } // Вторичный ключ
        public Group? Group { get; set; } // Навигационное свойство


        public int CharacterId { get; set; } // Вторичный ключ
        public Character? Character { get; set; } // Навигационное свойство
    }
}
