using System;
using System.Collections.Generic;
using VaxTrack_SDG_Project.VaxTrack.DAL;

namespace VaxTrack.BLL
{
    // Common contract for all user repositories
    public interface IUserRepository
    {
        string GetPasswordHash(string username);
    }

    // Adapter for DoctorRepository
    public class DoctorRepositoryAdapter : IUserRepository
    {
        private readonly DoctorRepository _doctorRepository;

        public DoctorRepositoryAdapter()
        {
            _doctorRepository = new DoctorRepository();
        }

        public string GetPasswordHash(string username)
        {
            return _doctorRepository.GetPasswordHash(username);
        }
    }

    // Adapter for AdminRepository
    public class AdminRepositoryAdapter : IUserRepository
    {
        private readonly AdminRepository _adminRepository;

        public AdminRepositoryAdapter()
        {
            _adminRepository = new AdminRepository();
        }

        public string GetPasswordHash(string username)
        {
            return _adminRepository.GetPasswordHash(username);
        }
    }

    public class AuthenticationService
    {
        private readonly Dictionary<string, IUserRepository> _repositories;

        public AuthenticationService()
        {
            // Map roles to repositories (easily extendable)
            _repositories = new Dictionary<string, IUserRepository>(StringComparer.OrdinalIgnoreCase)
            {
                { "Doctor", new DoctorRepositoryAdapter() },
                { "Admin", new AdminRepositoryAdapter() }
            };
        }

        public bool Login(string username, string password, string role)
        {
            try
            {
                // Input validation
                ValidateInputs(username, password, role);

                // Check if role exists
                if (!_repositories.TryGetValue(role, out IUserRepository repository))
                {
                    throw new ArgumentException($"Invalid role: {role}");
                }

                // Retrieve stored hash
                string storedHash = repository.GetPasswordHash(username);

                if (string.IsNullOrEmpty(storedHash))
                {
                    return false; // User not found
                }

                // Verify password
                return PasswordHasher.VerifyPassword(password, storedHash);
            }
            catch (ArgumentException ex)
            {
                // Handle known validation errors
                Console.WriteLine($"Validation Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                Console.WriteLine($"Authentication Error: {ex.Message}");
                return false;
            }
        }

        private void ValidateInputs(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role must be specified.");
        }
    }
}