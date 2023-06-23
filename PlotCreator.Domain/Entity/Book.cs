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
    public class Book
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Вторичный ключ
        public User? User { get; set; } // Навигационное свойство

        public int ModificatorId { get; set; } // Вторичный ключ
        public Access_Modificator? Modificator { get; set; } // Навигационное свойство

        [Required]
        public string? Title { get; set; }

        public int RatingId { get; set; } // Вторичный ключ
        public Rating? Rating { get; set; } // Навигационное свойство

        public int GenreId { get; set; } // Вторичный ключ
        public Genre? Genre { get; set; } // Навигационное свойство

        public int StatusId { get; set; } // Вторичный ключ
        public Book_Status? Status { get; set; } // Навигационное свойство



        [Column(TypeName = "NText")]
        public string? Description { get; set; }

        public string? Book_cover { get; set; }

		//Навигационные свойства для зависимых таблиц 
		//  |  |  |  |  |
		//  v  v  v  v  v
		public List<Episode>? Episodes { get; set; } = new();
		public List<Event>? Events { get; set; } = new();
		public List<Group>? Groups { get; set; } = new();


		[DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Book_Idea> Books_Ideas { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Book_Character> Books_Characters { get; set; } = new();
    }
}
