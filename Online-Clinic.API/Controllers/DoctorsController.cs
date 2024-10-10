using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using System.Numerics;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> _doctorRepository;
        public DoctorsController(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpPost("Add-Doctor")]
        public IActionResult AddDoctor(Doctor doctor)
        {
            _doctorRepository.AddEntity(doctor);
            //TODO handle exceptions
            return Ok();
        }

        [HttpPut("Update-Doctor/{id}")]
        public IActionResult UpdateDoctor(int id, Doctor doctor)
        {
            _doctorRepository.UpdateEntity(id, doctor);
            //TODO handle exceptions
            return Ok();
        }

        [HttpDelete("Delete-Doctor/{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            _doctorRepository.DeleteEntity(id);
            //TODO handle exceptions
            return Ok();
        }

        [HttpGet("Get-Doctor-By-Id/{id}")]
        public IActionResult GetDoctor(int id)
        {
            Doctor doctor = _doctorRepository.GetEntity(id);
            //TODO handle exceptions
            return Ok(doctor);
        }

        [HttpGet("Get-All-Doctors")]
        public IActionResult GetAllDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetEntities();
            //TODO handle exceptions
            return Ok(doctors);
        }
    }
}
