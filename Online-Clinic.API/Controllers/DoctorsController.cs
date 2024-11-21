using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Clinic.API.DTOs;
using Online_Clinic.API.Enums;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using System.Numerics;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly ICVService _cvService;

        public DoctorsController(IDoctorRepository doctorRepository, IMapper mapper, ICVService cvService)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _cvService = cvService;
        }

        [HttpPost("Add-Doctor")]
        public IActionResult AddDoctor(DoctorRequest doctorDto)
        {
            var doctor = _mapper.Map<Doctor>(doctorDto);
             _doctorRepository.AddEntity(doctor);
            return Ok();
        }

        [HttpPut("Update-Doctor/{id}")]
        public IActionResult UpdateDoctor(int id, DoctorRequest doctorDto)
        {
            var doctor = _mapper.Map<Doctor>(doctorDto);
            _doctorRepository.UpdateEntity(id, doctor);
            return Ok();
        }

        [HttpDelete("Delete-Doctor/{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            _doctorRepository.DeleteEntity(id);
            return Ok();
        }

        [HttpGet("Get-Doctor-By-Id/{id}")]
        [ResponseCache(Duration = 60)]
        public IActionResult GetDoctor(int id)
        {
            Doctor doctor = _doctorRepository.GetEntity(id);
            var doctorDto = _mapper.Map<DoctorResponse>(doctor);
            return Ok(doctorDto);
        }

        [HttpGet("Get-All-Doctors")]
        [ResponseCache(Duration = 60)]
        public IActionResult GetAllDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetEntities();
            var doctorDtos = _mapper.Map<List<DoctorResponse>>(doctors);
            return Ok(doctorDtos);
        }

        [HttpPost("Upload-Image/{doctorId}")]
        public IActionResult UploadImage(int doctorId, IFormFile file)
        {
            _doctorRepository.UploadImage(doctorId, file);
            return Ok();
        }

        [HttpGet("Get-Image/{doctorId}")]
        [ResponseCache(Duration = 60)]
        public IActionResult GetImage(int doctorId)
        {
            byte[] photoData = _doctorRepository.GetImage(doctorId);

            string fileType = "image/jpeg"; 
            var fileName = $"Doctor_{doctorId}.jpg"; 

            return File(photoData, fileType, fileName);
        }

        [HttpPost("Upload-CV/{doctorId}")]
        public IActionResult UploadCV(int doctorId, IFormFile file)
        {
            _doctorRepository.UploadCV(doctorId, file);
            return Ok();
        }

        [HttpGet("Get-CV/{doctorId}")]
        public IActionResult GetCV(int doctorId)
        {
            byte[] cvData = _doctorRepository.GetCV(doctorId);

            string fileType = "application/pdf";
            var fileName = $"Doctor_{doctorId}_CV.pdf";

            return File(cvData, fileType, fileName);
        }

        [HttpGet("Parse-CV/{doctorId}")]
        //[ResponseCache(Duration = 60)]
        public IActionResult ParseCV(int doctorId)
        {
            byte[] cvData = _doctorRepository.GetCV(doctorId);

            var parsedExperiences = _cvService.ParseExperienceFromPdf(cvData);
            return Ok(parsedExperiences);
        }

        [HttpGet("Get-Category-List")]
        [ResponseCache(Duration = 60)]
        public IActionResult GetCategoryList()
        {
            List<CategoryInfo> categoryInfos = _doctorRepository.GetCategoryList();
            return Ok(categoryInfos);
        }
    }
}
