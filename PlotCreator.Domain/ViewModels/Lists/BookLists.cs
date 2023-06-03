using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels.Lists
{
    public class BookLists
    {
        public List<Access_Modificator>? ModificatorList { get; set; }
        public List<Rating>? RatingList { get; set; }
        public List<Genre>? GenreList { get; set; }
        public List<Book_Status>? StatusList { get; set; }
    }
}
