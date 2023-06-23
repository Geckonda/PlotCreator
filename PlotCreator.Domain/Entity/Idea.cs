using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity.Multiple_Tables;

namespace PlotCreator.Domain.Entity
{
    public class Idea
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Вторичный ключ
        public User? User { get; set; } // Навигационное свойство

        [Required]
        public string? Topic { get; set; }

        [Required]
        public DateTime Data_Creation { get; set; }

        [Column(TypeName = "NText")]
        public string? Content { get; set; }

		//Навигационные свойства для зависимых таблиц 
		//  |  |  |  |  |
		//  v  v  v  v  v

		[DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Book_Idea> Books_Ideas { get; set; } = new();
    }
}
