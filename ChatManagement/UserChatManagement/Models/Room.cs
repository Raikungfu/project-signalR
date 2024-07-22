using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserChatManagement.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdminId { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public ApplicationUser Admin { get; set; }
        [NotMapped]
        public ApplicationUser User { get; set; }
        public ICollection<RoomUser> RoomUsers { get; set; }

        public ICollection<Message> Messages { get; set; }

        // Tin nhắn riêng tư trong phòng
        public ICollection<PrivateMessage> PrivateMessages { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}

