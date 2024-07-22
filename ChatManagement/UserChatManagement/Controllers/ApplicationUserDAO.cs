using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserChatManagement.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace UserChatManagement.Controllers
{
    public class AdminLoginResult
    {
        public SignInResult SignInResult { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
    }
    public class ApplicationUserDAO
    {
        private readonly AppDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserDAO(AppDbContext _dbContext, UserManager<ApplicationUser> userManager)
        {
            dbContext = _dbContext;
            _userManager = userManager;
        }


        // Create
        public async Task<IdentityResult> AddApplicationUser(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {

            }
            else
            {
                var errors = result.Errors;
            }
            return result;
        }

        private readonly UserManager<ApplicationUser> userManager;

        private static ApplicationUserDAO instance = null;
        public static readonly object instanceLock = new object();

        public async Task<List<ApplicationUser>> GetApplicationUsersAsync()
        {
            var users = await dbContext.ApplicationUsers
                .Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName ?? "Unknown",
                    Avatar = user.Avatar ?? "default-avatar.png"
                })
                .ToListAsync();

            return users;
        }


        public async Task<ApplicationUser> GetApplicationUserById(string userId)
        {
            return await dbContext.ApplicationUsers.FindAsync(userId);
        }

        public async Task<IdentityResult> UpdateApplicationUser(ApplicationUser user)
        {
            var existingUser = await dbContext.ApplicationUsers.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.Avatar = user.Avatar;

                dbContext.ApplicationUsers.Update(existingUser);
                var result = await dbContext.SaveChangesAsync();
                return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Update failed." });
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        /*
        public async Task<IdentityResult> DeleteApplicationUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return IdentityResult.Failed(new IdentityError { Description = "User ID cannot be null or empty." });
            }

            try
            {
                var user = await dbContext.ApplicationUsers
                    .Include(u => u.Messages)
                    .Include(u => u.PrivateMessagesSent)
                    .Include(u => u.PrivateMessagesReceived)
                    .Include(u => u.RoomUsers)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                dbContext.Messages.RemoveRange(user.Messages);
                dbContext.PrivateMessages.RemoveRange(user.PrivateMessagesSent);
                dbContext.PrivateMessages.RemoveRange(user.PrivateMessagesReceived);

                var rooms = await dbContext.Rooms
                    .Where(r => r.AdminId == userId || r.UserId == userId)
                    .Include(r => r.Messages)
                    .ToListAsync();

                foreach (var room in rooms)
                {
                    dbContext.Messages.RemoveRange(room.Messages);
                }

                dbContext.Rooms.RemoveRange(rooms);

                dbContext.ApplicationUsers.Remove(user);
                await dbContext.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }
        */



        public Task<List<ApplicationUser>> SearchApplicationUsers(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return dbContext.ApplicationUsers.Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName ?? "Unknown",
                    Avatar = user.Avatar ?? "default-avatar.png"
                }).ToListAsync();
            }
            else
            {
                return dbContext.ApplicationUsers.Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName ?? "Unknown",
                    Avatar = user.Avatar ?? "default-avatar.png"
                }).Where(u => u.UserName.Contains(searchTerm) || u.FullName.Contains(searchTerm))
                    .ToListAsync();
            }
        }

        // Filter
        public Task<List<ApplicationUser>> FilterApplicationUsers(Func<ApplicationUser, bool> predicate)
        {
            return Task.FromResult(dbContext.ApplicationUsers.Where(predicate).ToList());
        }

        public async Task<AdminLoginResult> LoginAdmin(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return new AdminLoginResult { SignInResult = SignInResult.Failed };
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new AdminLoginResult { SignInResult = SignInResult.Failed };
            }

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return passwordVerificationResult == PasswordVerificationResult.Success ? new AdminLoginResult
            {
                SignInResult = SignInResult.Success,
                UserName = user.UserName,
                FullName = user.FullName,
                AvatarUrl = user.Avatar
            } : new AdminLoginResult { SignInResult = SignInResult.Failed };
        }

        public async Task UpdatePasswordAsync(string email, string newPassword)
        {
            var user = await dbContext.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, newPassword);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Người dùng không tồn tại.");
            }
        }
    }
}
