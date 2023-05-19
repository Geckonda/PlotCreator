using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }

        public Book? Book { get; set; } 

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Parent { get; set; }

        public List<Group_Character> Groups_Characters { get; set; } = new();
        public List<Group_Event> Groups_Events { get; set; } = new();
    }
}
