using System;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.BLL
{
    public class VaccineService
    {
        private readonly VaccinationRepository _repository;

        public VaccineService()
        {
            _repository = new VaccinationRepository();
        }

        public void AddVaccine(VaccineCreateDto dto)
        {
            Validate(dto);
            _repository.AddVaccine(dto);
        }

        private void Validate(VaccineCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.VaccineName))
                throw new Exception("Vaccine name is required.");
        }
    }
}