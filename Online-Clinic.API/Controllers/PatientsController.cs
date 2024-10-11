using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Online_Clinic.API.Repositories.SQL;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;   
        }

        [HttpPost("Add-Patient")]
        public IActionResult AddPatient(Patient patient)
        {
            _patientRepository.AddEntity(patient);
            return Ok();
        }

        [HttpPut("Update-Patient/{id}")]
        public IActionResult UpdatePatient(int id, Patient patient)
        {
            _patientRepository.UpdateEntity(id, patient);
            return Ok();
        }

        [HttpDelete("Delete-Patient/{id}")]
        public IActionResult DeletePatient(int id)
        {
            _patientRepository.DeleteEntity(id);
            return Ok();
        }

        [HttpGet("Get-Patient-By-Id/{id}")]
        public IActionResult GetPatient(int id)
        {
            Patient patient = _patientRepository.GetEntity(id);
            return Ok(patient);
        }

        [HttpGet("Get-All-Patients")]
        public IActionResult GetAllPatients()
        {
            List<Patient> patients = _patientRepository.GetEntities();
            return Ok(patients);
        }
    }
}
