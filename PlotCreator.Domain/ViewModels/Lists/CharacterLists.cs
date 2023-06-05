using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels.Lists
{
	public class CharacterLists
	{
		public List<Worldview>? Worldviews { get; set; }
		public List<Group>? Groups { get; set; }
		public List<Episode>? Episodes { get; set; }
		public List<Event>? Events { get; set; }
	}
}
