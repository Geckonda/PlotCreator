﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System.ComponentModel.DataAnnotations;

namespace PlotCreator.Domain.Entity
{
    public class Episode
    {
        public int Id { get; set; }

        public int BookId { get; set; } // Вторичный ключ
        public Book? Book { get; set; } // Навигационное свойство
		[Required]
		public string? Heading { get; set; }
		public int Position { get; set; }

		[Column(TypeName = "NText")]
        public string? Content { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Episode_Event> Episodes_Events { get; set; } = new();

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Episode_Character> Episodes_Characters { get; set; } = new();
    }
}
