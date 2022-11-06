﻿using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }

    }
}
