using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Clinic.API.DTOs;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientsController(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;   
            _mapper = mapper;
        }

        [HttpPost("Add-Patient")]
        public IActionResult AddPatient(PatientRequest patientDto)
        {
            var patient = _mapper.Map<Patient>(patientDto);
            _patientRepository.AddEntity(patient);
            return Ok();
        }

        [HttpPut("Update-Patient/{id}")]
        public IActionResult UpdatePatient(int id, PatientRequest patientDto)
        {
            var patient = _mapper.Map<Patient>(patientDto);
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
            var patientDto = _mapper.Map<PatientResponse>(patient);
            return Ok(patientDto);
        }

        [HttpGet("Get-All-Patients")]
        public IActionResult GetAllPatients()
        {
            List<Patient> patients = _patientRepository.GetEntities();
            var patientDtos = _mapper.Map<List<PatientResponse>>(patients);
            return Ok(patientDtos);
        }
    }
}
