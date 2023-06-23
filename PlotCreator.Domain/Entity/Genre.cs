using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity
{
    public class Genre
    {
        public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }

		//Навигационные свойства для зависимых таблиц 
		//  |  |  |  |  |
		//  v  v  v  v  v
		public List<Book> Books { get; set; } = new();
    }
}
