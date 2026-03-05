

using System;

namespace MobileApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public int Bought { get; set; }
        public int Used { get; set; }
        public int Wasted { get; set; }
    }
}
