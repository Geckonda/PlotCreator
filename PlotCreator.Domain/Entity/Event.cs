using System;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Event : EntityBase
    {
        public DateOnly? Beginning { get; set; }
        public DateOnly? Ending { get; set; }
        public bool ChekhovsGun { get; set; }
    }
}
