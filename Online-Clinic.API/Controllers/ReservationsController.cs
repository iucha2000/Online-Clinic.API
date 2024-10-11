using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Online_Clinic.API.Repositories.SQL;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationsRepository;

        public ReservationsController(IReservationRepository reservationsRepository)
        {
            _reservationsRepository = reservationsRepository;
        }

        [HttpPost("Add-Reservation")]
        public IActionResult AddReservation(Reservation reservation)
        {
            _reservationsRepository.AddEntity(reservation);
            //TODO handle exceptions
            return Ok();
        }

        [HttpPut("Update-Reservation/{id}")]
        public IActionResult UpdateReservation(int id, Reservation reservation)
        {
            _reservationsRepository.UpdateEntity(id, reservation);
            //TODO handle exceptions
            return Ok();
        }

        [HttpDelete("Delete-Reservation/{id}")]
        public IActionResult DeleteReservation(int id)
        {
            _reservationsRepository.DeleteEntity(id);
            //TODO handle exceptions
            return Ok();
        }

        [HttpGet("Get-Reservation-By-Id/{id}")]
        public IActionResult GetReservation(int id)
        {
            Reservation reservation = _reservationsRepository.GetEntity(id);
            //TODO handle exceptions
            return Ok(reservation);
        }

        [HttpGet("Get-All-Reservations")]
        public IActionResult GetAllReservations()
        {
            List<Reservation> reservations = _reservationsRepository.GetEntities();
            //TODO handle exceptions
            return Ok(reservations);
        }
    }
}
