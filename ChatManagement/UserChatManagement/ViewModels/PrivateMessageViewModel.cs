using System;

namespace UserChatManagement.ViewModels
{
    public class PrivateMessageViewModel
    {
        public int Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Room { get; set; }
        public string Avatar { get; set; }
    }
}
