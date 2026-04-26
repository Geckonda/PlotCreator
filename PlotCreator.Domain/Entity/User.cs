using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PlotCreator.Domain.Entity
{
    [Index(nameof(Login), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        [Required]
        public string? Nickname { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? Avatar { get; set; }

        public List<World> Worlds { get; set; } = new();
    }
}
