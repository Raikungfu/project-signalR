﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UserChatManagement.ViewModels
{
    public class UploadViewModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string RoomName { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
