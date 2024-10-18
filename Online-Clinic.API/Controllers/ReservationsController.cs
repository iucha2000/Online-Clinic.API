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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationsRepository;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationRepository reservationsRepository, IMapper mapper)
        {
            _reservationsRepository = reservationsRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("Add-Reservation")]
        public IActionResult AddReservation(ReservationDto reservationDto)
        {
            var reservation = _mapper.Map<Reservation>(reservationDto);
            _reservationsRepository.AddEntity(reservation);
            return Ok();
        }

        [Authorize]
        [HttpPut("Update-Reservation/{id}")]
        public IActionResult UpdateReservation(int id, ReservationDto reservationDto)
        {
            var reservation = _mapper.Map<Reservation>(reservationDto);
            _reservationsRepository.UpdateEntity(id, reservation);
            return Ok();
        }

        [Authorize]
        [HttpDelete("Delete-Reservation/{id}")]
        public IActionResult DeleteReservation(int id)
        {
            _reservationsRepository.DeleteEntity(id);
            return Ok();
        }

        [HttpGet("Get-Reservation-By-Id/{id}")]
        public IActionResult GetReservation(int id)
        {
            Reservation reservation = _reservationsRepository.GetEntity(id);
            return Ok(reservation);
        }

        [HttpGet("Get-All-Reservations")]
        public IActionResult GetAllReservations()
        {
            List<Reservation> reservations = _reservationsRepository.GetEntities();
            return Ok(reservations);
        }

        [HttpGet("Get-Reservations-By-Patient/{patientId}")]
        public IActionResult GetReservationsByPatientId(int patientId)
        {
            List<Reservation> reservations = _reservationsRepository.GetByPatientId(patientId);
            return Ok(reservations);
        }

        [HttpGet("Get-Reservations-By-Doctor/{doctorId}")]
        public IActionResult GetReservationsByDoctorId(int doctorId)
        {
            List<Reservation> reservations = _reservationsRepository.GetByDoctorId(doctorId);
            return Ok(reservations);
        }
    }
}
