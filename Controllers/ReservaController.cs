using API.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ReservaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllReservations()
        {
            return Ok(await _dataContext.Reserva.Include(p => p.Livro)?.ToListAsync());
        }

        [HttpPost()]
        public async Task<ActionResult> AddReservation([FromBody] Reserva reserva)
        {
            await _dataContext.Reserva.AddAsync(reserva);
            await _dataContext.SaveChangesAsync();
            return Ok(reserva);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            var reserva = await _dataContext.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            _dataContext.Reserva.Remove(reserva);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patchreserva(
            int id,
            [FromBody] JsonPatchDocument<Reserva> reserva
        )
        {
            if (reserva == null)
            {
                return BadRequest();
            }

            var reservabyid = await _dataContext.Reserva.FindAsync(id);

            if (reservabyid == null)
            {
                return NotFound();
            }

            reserva.ApplyTo(reservabyid, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ReservaExists(int id)
        {
            return _dataContext.Reserva.Any(e => e.ReservaId == id);
        }
    }
}
