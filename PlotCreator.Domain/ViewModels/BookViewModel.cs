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
    public class BookViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int Access_ModificatorId { get; set; } 
        public string? Title { get; set; }
        public int RatingId { get; set; }
        public int GenreId { get; set; }
        public int Book_StatusId { get; set; } 
        public string? Description { get; set; }
        public string? Book_cover { get; set; }
    }
}
