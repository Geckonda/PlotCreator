using PlotCreator.Domain.Filters.FilterFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Filters
{
	public class EventFilter
	{
		public int BookId { get; set; }
		public DateFilter? Date { get; set; }
		public bool CheckhovsGun { get; set; }
		public bool IsHidden { get; set; }
		public CheckBoxFilter? Groups { get; set; } = new();
	}
}
