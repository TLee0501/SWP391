using System;
namespace BusinessObjects.RequestModel
{
    public class CreateAccountRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid RoleId { get; set; }
    }
}

