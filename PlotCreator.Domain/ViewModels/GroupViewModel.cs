using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
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

		public List<Character> Characters{ get; set; } = new();
		public List<Event> Events { get; set; } = new();
	}
}
