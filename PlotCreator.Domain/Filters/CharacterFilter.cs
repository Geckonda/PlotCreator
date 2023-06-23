using PlotCreator.Domain.Filters.FilterFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Filters
{
	public class CharacterFilter
	{
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateFilter? Date { get; set; }
		public string[] Gender { get; set; } = new string[0];
		public NumericFilter? Height { get; set; }
		public NumericFilter? Weight { get; set; }
		public CheckBoxFilter? Worldviews { get; set; } = new();
		public CheckBoxFilter? Groups { get; set; } = new();

	}
}
