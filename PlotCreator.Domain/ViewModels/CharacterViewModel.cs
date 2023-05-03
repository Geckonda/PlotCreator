using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PlotCreator.Domain.ViewModels
{
    public class CharacterViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; } 

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


        public int WorldviewId { get; set; } 
        public Worldview? Worldview { get; set; } 
        public List<Worldview>? Worldviews { get; set; } 

        public string? Picture { get; set; }
        public IFormFile? PictureImage { get; set; }
        public DateTime Deathday { get; set; }

    }
}
