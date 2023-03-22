﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Entity
{
    public class Rating
    {
        public int Id { get; set; }
        public string? Rate { get; set; }

        //Навигационные свойства для зависимых таблиц 
        //  |  |  |  |  |
        //  v  v  v  v  v
        public List<Book> Books { get; set; } = new();
    }
}
