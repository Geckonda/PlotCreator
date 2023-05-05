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

        public int Access_ModificatorId { get; set; } // Вторичный ключ
        public Access_Modificator? Access_Modificator { get; set; } // Навигационное свойство

        [NotMapped]
		public List<Access_Modificator>? Access_Modificators { get; set; }

		[Required]
        public string? Title { get; set; }

        public int RatingId { get; set; } // Вторичный ключ
        public Rating? Rating { get; set; } // Навигационное свойство

        [NotMapped]
		public List<Rating>? Ratings { get; set; }

		public int GenreId { get; set; } // Вторичный ключ
        public Genre? Genre { get; set; } // Навигационное свойство

        [NotMapped]
		public List<Genre>? Genres { get; set; }

		public int Book_StatusId { get; set; } // Вторичный ключ
        public Book_Status? Book_Status { get; set; } // Навигационное свойство

        [NotMapped]
		public List<Book_Status>? Book_Statuses { get; set; }



		[Column(TypeName = "Ntext")]
        public string? Description { get; set; }

        public string? Book_cover { get; set; }

        [NotMapped]
        public List<Idea>? Ideas { get; set; }
        [NotMapped]
        public List<Character>? Characters { get; set; }
        [NotMapped]
        public List<Episode>? Episodes { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Book_Idea> Books_Ideas { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Book_Character> Books_Characters { get; set; } = new();
    }
}
