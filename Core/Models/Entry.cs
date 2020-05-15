using System;

namespace Core.Models
{
    public class Entry : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdParticipant { get; set; }
        public Guid IdTask { get; set; }
    }
}