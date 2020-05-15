using System;
using System.Collections.Generic;

namespace Core.Models
 {
    public class ContestTask:IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Distance Distance{ get; set; }
        public Style Style{ get; set; }
        public List<Participant> Participants{ get; set; }=new List<Participant>();
        public int NoOfParticipants{ get; set; }
    }
}