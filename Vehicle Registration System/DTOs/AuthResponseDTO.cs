﻿namespace Vehicle_Registration_System.DTOs
{
    public class AuthResponseDTO
    {
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;

    }
}
