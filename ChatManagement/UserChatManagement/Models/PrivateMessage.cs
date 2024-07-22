using System;


namespace UserChatManagement.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }

        public string FromUserId { get; set; }
        public virtual ApplicationUser FromUser { get; set; }

        public string ToUserId { get; set; }
        public virtual ApplicationUser ToUser { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public int? ToRoomId { get; set; }
        public virtual Room ToRoom { get; set; }
    }
}
