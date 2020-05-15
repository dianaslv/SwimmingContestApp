using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Participant : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Age { get; set; }
        public List<ContestTask> TaskList { get; set; } = new List<ContestTask>();
    }
}