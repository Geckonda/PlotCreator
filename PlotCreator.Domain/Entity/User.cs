using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PlotCreator.Domain.Entity
{
    [Index("Login", IsUnique = true)]
    [Index("Email", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public int roleId { get; set; } // Внешний ключ
        public Role? Role { get; set; } // Навигационное свойство

        [Required]
        public string? Nickname { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        public string? Avatar { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v
        public List<Book> Books { get; set; } = new();
        public List<Idea> Ideas { get; set; } = new();
        public List<Character> Characters { get; set; } = new();
    }
}
