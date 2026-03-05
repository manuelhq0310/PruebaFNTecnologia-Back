using Middleware.Exceptions;
using UserService.Domain.Enums;

namespace UserService.Domain.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User(string name, string email, string passwordHash, UserRole role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public static User Create(string name, string email, string passwordHash, UserRole role)
        {
            if (role != UserRole.Admin && role != UserRole.User)
                throw new DomainException("Invalid user role.");

            return new User(name, email, passwordHash, role);
        }

        public void ChangeName(string name)
        {
            SetName(name);
        }

        public void ChangeRole(UserRole role)
        {
            if (role != UserRole.Admin && role != UserRole.User)
                throw new DomainException("Invalid role.");

            Role = role;
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainException("Password cannot be empty.");

            PasswordHash = newPasswordHash;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required.");

            Name = name.Trim();
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email is required.");

            Email = email.Trim().ToLower();
        }

        private void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("Password hash is required.");

            PasswordHash = passwordHash;
        }
    }
}
