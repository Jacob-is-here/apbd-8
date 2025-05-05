using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {

        private readonly ITripsService _tripsService;

        public ClientsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetCos(int id)
        {
            
            var person = await _tripsService.GetIdTrips(id);

            if ( person == null || !person.Any() )
            {
                return NotFound(new { Message = $"No trips found for client with ID {id}." });
            }
            return Ok(person);
            
        }

        [HttpGet]
        public async Task<IActionResult> people()
        {
            var person = await _tripsService.Take();
            return Ok(person);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Add(OnlyPerson person)
        {
            
            await _tripsService.NewPerson(person);
            return Created();
        }

        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> Change(int id , int tripId)
        {
            var osoba = await _tripsService.CzyKlientIstnieje(id);
            if (osoba == 0)
            {
                return NotFound($"nie istnieje osoba o podanym id :{id}");
            }
            var trip = await _tripsService.CzyTripIstnieje(tripId);
            if (trip == 0)
            {
                return NotFound($"nie istnieje trip o podanym tripid :{tripId}");
            }

            await _tripsService.Zmianka(id, tripId);
            
            
            return Created();
        }

        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> Usun(int id, int tripId)
        {
            var osoba = await _tripsService.CzyKlientIstnieje(id);
            if (osoba == 0)
            {
                return NotFound($"nie istnieje osoba o podanym id :{id}");
            }
            var trip = await _tripsService.CzyTripIstnieje(tripId);
            if (trip == 0)
            {
                return NotFound($"nie istnieje trip o podanym tripid :{tripId}");
            }
            
            
            
            await _tripsService.Delete(id, tripId);
            
            return Ok();
        }
    }
}