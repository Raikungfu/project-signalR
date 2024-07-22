using System.Windows.Forms;
using UserChatManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using UserChatManagement.ViewModels;


namespace UserChatManagement.Controllers
{
    public class RoomDAO
    {
        private readonly AppDbContext _context;

        public RoomDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Room> StartChat(string userName, string adminUserName)
        {
            var admin = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == adminUserName);
            if (admin == null)
            {
                return null;
            }

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }

            var roomName = GenerateRoomName(admin.UserName, user.UserName);
            var room = await _context.Rooms.Include(r => r.Messages)
                                            .FirstOrDefaultAsync(r => r.Name == roomName);

            if (room == null)
            {
                room = new Room
                {
                    Name = roomName,
                    AdminId = admin.Id,
                    Description = "private",
                    UserId = user.Id,
                };
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
            }

            return room;
        }

        private string GenerateRoomName(string adminName, string userName)
        {
            var sortedUsernames = new List<string> { adminName, userName };
            sortedUsernames.Sort();
            return $"private_{sortedUsernames[0]}_{sortedUsernames[1]}";
        }

        public async Task<MessageViewModel> SaveMessageAsync(string roomName, string userName, string content)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);

            if (room == null || user == null)
            {
                return null;
            }

            var msg = new Models.Message()
            {
                Content = Regex.Replace(content, @"<.*?>", string.Empty),
                FromUser = user,
                ToRoom = room,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            var createdMessage = new MessageViewModel
            {
                Id = msg.Id,
                Content = msg.Content,
                Timestamp = msg.Timestamp,
                FromUserName = msg.FromUser.UserName,
                FromFullName = msg.FromUser.FullName,
                Room = room.Name,
                Avatar = msg.FromUser.Avatar
            };

            return createdMessage;
        }
    }
}
