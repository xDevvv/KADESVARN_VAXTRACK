using System;
using System.Collections.Generic;
using System.Text;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.BLL
{
    internal class DoctorService
    {
        private readonly DoctorRepository _repository = new DoctorRepository();

        public void AddDoctor(Doctor dto)
        {
            Validate(dto);

            // Hash password centrally in BLL
            dto.Password = PasswordHasher.HashPassword(dto.Password);

            _repository.AddDoctor(dto);
        }

        private void Validate(Doctor dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new Exception("Username is required.");
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Password is required.");
            if (!IsValidEmail(dto.Email))
                throw new Exception("Invalid email.");
        }

        private bool IsValidEmail(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }
    }
}
