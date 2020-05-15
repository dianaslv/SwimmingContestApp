using System;

namespace Core.Models
{
    public class User : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Password { get; set; }
    }
}