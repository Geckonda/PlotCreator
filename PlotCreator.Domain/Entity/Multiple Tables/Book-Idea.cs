using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity.Multiple_Tables
{
    [Table("Book-Idea")]
    public class Book_Idea
    {
        public int Id { get; set; }

        public int BookId { get; set; } // Вторичный ключ
        public Book? Book { get; set; } // Навигационное свойство


        public int IdeaId { get; set; } // Вторичный ключ
        public Idea? Idea { get; set; } // Навигационное свойство

        public DateTime? DateCreation { get; set; }
    }
}
