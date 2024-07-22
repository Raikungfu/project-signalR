namespace UserChatManagement.Models
{
    public class RoomUser
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
