using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserChatManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? Avatar { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<RoomUser> RoomUsers { get; set; }
        public ICollection<PrivateMessage> PrivateMessagesSent { get; set; }

        public ICollection<PrivateMessage> PrivateMessagesReceived { get; set; }
    }
}
