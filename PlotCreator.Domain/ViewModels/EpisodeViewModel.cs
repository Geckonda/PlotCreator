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
    public class EpisodeViewModel
    {
        public int Id { get; set; }
        public Book? Book { get; set; }

        public string? Heading { get; set; }
        public int Position { get; set; }

        public string? Content { get; set; }


        //

        public List<Episode_Event> Episodes_Events { get; set; } = new();

        public List<Episode_Character> Episodes_Characters { get; set; } = new();
    }
}
