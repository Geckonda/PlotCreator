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
using Microsoft.AspNetCore.Http;

namespace PlotCreator.Domain.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public Access_Modificator? Modificator { get; set; }
        public List<Access_Modificator>? ModificatorList { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string? Title { get; set; }
        public Rating? Rating { get; set; }
        public List<Rating>? RatingList { get; set; }
        public Genre? Genre { get; set; }
        public List<Genre>? GenreList { get; set; }
        public Book_Status? Status { get; set; }
        public List<Book_Status>? StatusList { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string? Description { get; set; }
        public string? Book_cover { get; set; }
        public IFormFile? Book_coverImage { get; set; }
        public List<Idea>? Ideas { get; set; }
        public List<Character>? Characters { get; set; }
        public List<Episode>? Episodes { get; set; }
    }
}
