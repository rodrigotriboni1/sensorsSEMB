﻿namespace sensoresMAUISEMB.Models
{
    public class User
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
