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
        public string Nickname { get; set; } = string.Empty;

        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Email {  get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
    }
}
