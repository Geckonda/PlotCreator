using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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

		public List<Event> Events { get; set; } = new();
		public List<Character>Characters { get; set; } = new();

        //Листы под персонажей и события
        public List<Event> EventList { get; set; } = new();
        public List<Character> CharacterList { get; set; } = new();
    }
}
