using Microsoft.AspNetCore.Http;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels
{
	public class CharacterViewModel
	{
		public int Id { get; set; }
		public User? User { get; set; }
		public string? Name { get; set; }
		public DateTime Birthday { get; set; }
		public string? Gender { get; set; }
		public int Height { get; set; }
		public int Weight { get; set; }
		public string? Personality { get; set; }
		public string? Appearance { get; set; }
		public string? Goals { get; set; }
		public string? Motivation { get; set; }
		public string? History { get; set; }
		public string? Conflict { get; set; }

		public Worldview? Worldview { get; set; }
		public List<Worldview>? Worldviews { get; set; }
		public List<Group>? Groups { get; set; }
		public List<Episode>? Episodes { get; set; }
		public List<Event>? Events { get; set; }
		public List<Book>? Books { get; set; }

		public string? Picture { get; set; }
		public IFormFile? PictureImage { get; set; }
		public DateTime Deathday { get; set; }

		//Листы под события и эпизоды
		public List<Group>? GroupList { get; set; }
		public List<Event>? EventList { get; set; }
		public List<Episode>? EpisodeList { get; set; }
		public int[]? checkedGroups { get; set; }
		public int[]? checkedEvents { get; set; }
		public int[]? checkedEpisodes { get; set; }
	}
}
