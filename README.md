# VaxTrack: Community Immunization & Health Records System

## 🎯 SDG Alignment
**Target SDG:** Goal 3 – Good Health and Well-being  
VaxTrack addresses the need for effective vaccination tracking and secure management of patient health records, helping communities maintain better immunization coverage and improve public health outcomes.

---

## ❗ Problem Statement
In many communities, healthcare providers face challenges in **tracking immunization schedules** and maintaining accurate, secure records for patients. Overdue vaccinations are common due to manual record-keeping and lack of reminders, which increases the risk of preventable diseases.

VaxTrack solves this problem by providing a **centralized, digital system** for managing patients’ health and immunization records, alerting healthcare providers of overdue vaccinations, and providing analytics to support preventive care.

---

## 📝 Project Description
VaxTrack is a **C# WinForms application** designed using **N-Tier architecture**:

- **Presentation Layer (UI):** Windows Forms for Doctors/Admins to interact with the system.  
- **Business Logic Layer (BLL):** Handles vaccination alerts, analytics, and validation.  
- **Data Access Layer (DAL):** Manages secure interaction with the database using SQLite.  
- **Models:** Represents database entities such as Admins, Doctors, Patients, and Immunizations.

The architecture ensures **modularity, scalability, and maintainability**, and separates concerns between UI, logic, and data. SQLite is used for **lightweight, portable database management**.

### Features
- **Admin & Doctor Login:** Secure authentication with password hashing.  
- **Patient & Immunization Management:** CRUD operations for patient and vaccination records.  
- **Vaccination Alerts:** Notifications for overdue vaccinations.  
- **Health Analytics Dashboard:** Visual charts showing vaccinated vs unvaccinated populations.  
- **Export Records:** Digital backup in JSON format.

---

## 👥 Contributors

| Name | Role / Module |
|------|---------------|
| Kennev Francisco | Project Lead, UI/WinForms, Business Logic Layer (Services) |
| Sean Howard Surigao | Data Access Layer (Repositories) |
| Diane Varquez | Models & Database Schema (SQLite) |
| Reyna Francisco | Documentation |

## ⚙️ Installation & Usage

1. **Clone the repository**
```bash
git clone https://github.com/[YourUsername]/VaxTrack.git
