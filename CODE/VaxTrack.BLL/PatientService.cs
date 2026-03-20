using System;
using System.Collections.Generic;
using System.Text;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.BLL
{
    internal class PatientService
    {
        private readonly PatientRepository _repository;
        private readonly VaccinationScheduleRepository _scheduleRepo = new VaccinationScheduleRepository();

        public PatientService()
        {
            _repository = new PatientRepository();
        }

        public void AddPatient(PatientCreateDto dto)
        {
            Validate(dto);

            // Example business rule
            if (dto.vaccineId > 0)
                dto.isVaccinated = true;

            _repository.AddPatient(dto);
        }

        private void Validate(PatientCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.firstName))
                throw new Exception("First name is required.");

            if (string.IsNullOrWhiteSpace(dto.lastName))
                throw new Exception("Last name is required.");

            if (dto.age <= 0)
                throw new Exception("Invalid age.");

            if (dto.doctorId <= 0)
                throw new Exception("Invalid doctor.");
        }

        public bool ScheduleVaccination(int patientId, DateTime date, string vaccine)
        {
            try 
            {
                // Rule: Check if patient already has a "Completed" vaccination record
                if (_repository.IsPatientVaccinated(patientId))
                {
                    throw new Exception("Patient is already vaccinated and cannot be rescheduled.");
                }

                // Rule: Ensure appointment is in the future
                if (date < DateTime.Now)
                {
                    throw new Exception("Cannot schedule appointments in the past.");
                }

                _scheduleRepo.CreateAppointment(patientId, date, vaccine);
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return false;
            }
           
        }
    }
}
