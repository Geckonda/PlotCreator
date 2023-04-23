using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class IdeaViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; } 

        public string? Topic { get; set; }

        public DateTime Data_Creation { get; set; }

        public string? Content { get; set; }
    }
}
