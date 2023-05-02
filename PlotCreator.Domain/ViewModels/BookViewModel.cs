using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PlotCreator.Domain.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int Access_ModificatorId { get; set; } 
        public string? Access_Modificator { get; set; }
        public List<Access_Modificator>? Access_Modificators { get; set; }
        public string? Title { get; set; }
        public int RatingId { get; set; }
        public string? Rating { get; set; }
        public List<Rating>? Ratings { get; set; }
        public int GenreId { get; set; }
        public string? Genre { get; set; }
        public List<Genre>? Genres { get; set; }
        public int Book_StatusId { get; set; }
        public string? Book_Status { get; set; }
        public List<Book_Status>? Book_Statuses { get; set; }
        public string? Description { get; set; }
        public string? Book_cover { get; set; }
        public IFormFile? Book_coverImage { get; set; }
		public List<Idea>? Ideas { get; set; }
		public List<Character>? Characters { get; set; }
		public List<Episode>? Episodes { get; set; }
	}
}
