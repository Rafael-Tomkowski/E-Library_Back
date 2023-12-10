using API.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimoController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public EmprestimoController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllLoans()
        {
            return Ok(await _dataContext.Emprestimo.ToListAsync());
        }

        [HttpPost()]
        public async Task<ActionResult> AddLoan([FromBody] Emprestimo emprestimo)
        {
            await _dataContext.Emprestimo.AddAsync(emprestimo);
            await _dataContext.SaveChangesAsync();
            return Ok(emprestimo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLoan(int id)
        {
            var emprestimo = await _dataContext.Emprestimo.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            _dataContext.Emprestimo.Remove(emprestimo);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patchemprestimo(
            int id,
            [FromBody] JsonPatchDocument<Emprestimo> emprestimo
        )
        {
            if (emprestimo == null)
            {
                return BadRequest();
            }

            var emprestimobyid = await _dataContext.Emprestimo.FindAsync(id);

            if (emprestimobyid == null)
            {
                return NotFound();
            }
            
            emprestimo.ApplyTo(emprestimobyid, ModelState); 

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
                if (!EmprestimoExists(id))
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

        private bool EmprestimoExists(int id)
        {
            return _dataContext.Emprestimo.Any(e => e.EmprestimoId == id);
        }
    }
}