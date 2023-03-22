using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity.Multiple_Tables
{
    [Table("Group-Event")]
    public class Group_Event
    {
        public int Id { get; set; }

        public int GroupId { get; set; } // Вторичный ключ
        public Group? Group { get; set; } // Навигационное свойство


        public int EventId { get; set; } // Вторичный ключ
        public Event? Event { get; set; } // Навигационное свойство
    }
}
