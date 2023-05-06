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
    public class EventViewModel
    {

        public int Id { get; set; }

        public Book? Book { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
        public bool ChekhovsGun { get; set; }
        public bool IsHidden { get; set; }
        public string? Icon { get; set; }
        public string? Colour { get; set; }


        //  |  |  |  |  |
        //  v  v  v  v  v

        public List<Episode_Event> Episodes_Events { get; set; } = new();

        public List<Event_Character> Events_Characters { get; set; } = new();

        public List<Group_Event> Groups_Events { get; set; } = new();
    }
}
