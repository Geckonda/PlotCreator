using System;
using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Character : EntityBase
    {
        public DateOnly? Birthday { get; set; }
        public DateOnly? Deathday { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        public int? Height { get; set; }
        public int? Weight { get; set; }

        public string? Personality { get; set; }
        public string? Appearance { get; set; }
        public string? Conflict { get; set; }
        public string? Goals { get; set; }
        public string? Motivation { get; set; }
        public string? History { get; set; }

        [MaxLength(500)]
        public string? PictureUrl { get; set; }
    }
}
