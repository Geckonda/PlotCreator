using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity
{
    [Index("Login", IsUnique = true)]
    [Index("Email", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public int roleId { get; set; }
        public Role? Role { get; set; }

        [Required]
        public string? Nickname { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Email {  get; set; }

        [Required]
        public string? Password { get; set; }
        public string? Avatar { get; set; }
    }
}
