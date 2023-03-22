using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity.Multiple_Tables
{
    [Table("Book-Character")]
    public class Book_Character
    {
        public int Id { get; set; }

        public int BookId { get; set; } // Вторичный ключ
        public Book? Book { get; set; } // Навигационное свойство


        public int CharacterId { get; set; } // Вторичный ключ
        public Character? Character { get; set; } // Навигационное свойство
    }
}
