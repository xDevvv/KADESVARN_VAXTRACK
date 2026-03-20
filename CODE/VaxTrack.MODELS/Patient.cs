using System;
using System.Collections.Generic;
using System.Text;

namespace VaxTrack_SDG_Project.VaxTrack.MODELS
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? BirthDate { get; set; }          
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public int DoctorId { get; set; }
        public string? VaccineId { get; set; }
        public int? IsVaccinated { get; set; }         // 0 or 1
        public string? CreatedAt { get; set; }         // or DateTime
    }

    // ───────────────────────────────────────────────
    // DTO for creating a new patient (used in repositories/services/forms)
    // ───────────────────────────────────────────────
    public class PatientCreateDto
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public int age { get; set; }
        public string? gender { get; set; }
        public string? address { get; set; }
        public string birthDate { get; set; }
        public int doctorId { get; set; }
        public string contactNumber { get; set; }
        public string vaccineType { get; set; }
        public bool isVaccinated { get; set; }
        public int vaccineId { get; set; }

    }

    public class PatientUpdateDto
    {
        public int patientId { get; set; }              // Required to identify the patient
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public int age { get; set; }
        public string birthDate { get; set; }
        public string? gender { get; set; }
        public string? address { get; set; }
        
        public int vaccineId { get; set; }
        public bool isVaccinated { get; set; }
        public string contactNumber { get; set; } = string.Empty;
    }
}

public class ImportPatient
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string BirthDate { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public int DoctorID { get; set; }
    public int VaccineID { get; set; }
    public int IsVaccinated { get; set; }
    public string CreatedAt { get; set; }
}

