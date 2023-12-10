using API.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsuarioController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //
        [HttpGet()]
        public async Task<ActionResult> GetAllUsers()
        {
            return Ok(await _dataContext.Usuario.ToListAsync());
        }

        [HttpPost()]
        public async Task<ActionResult> AddUser([FromBody] Usuario usuario)
        {
            await _dataContext.Usuario.AddAsync(usuario);
            await _dataContext.SaveChangesAsync();
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var usuario = await _dataContext.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _dataContext.Usuario.Remove(usuario);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUsuario(
            int id,
            [FromBody] JsonPatchDocument<Usuario> usuario
        )
        {
            if (usuario == null)
            {
                return BadRequest();
            }

            var usuariobyid = await _dataContext.Usuario.FindAsync(id);

            if (usuariobyid == null)
            {
                return NotFound();
            }

            usuario.ApplyTo(usuariobyid, ModelState);

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
                if (!UsuarioExists(id))
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

        private bool UsuarioExists(int id)
        {
            return _dataContext.Usuario.Any(e => e.UsuarioId == id);
        }
    }
}
