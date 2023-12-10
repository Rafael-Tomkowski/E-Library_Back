using API.Data;
using LibApi;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public LivroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllBooks()
        {
            return Ok(await _dataContext.Livro.Where(r => r.Retirado == false).ToListAsync());
        }

        [HttpGet("Emprestimos")]
        public async Task<ActionResult> GetAllBorrowed()
        {
            return Ok(await _dataContext.Livro.Where(r => r.Retirado == true).ToListAsync());
        }

        [HttpPut("Emprestimo/{id}")]
        public async Task<ActionResult> CreateEmprestimo(int id)
        {
            var livro = await _dataContext.Livro.FindAsync(id);

            livro.Retirado = !livro.Retirado;

            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var livro = await _dataContext.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            return Ok(livro);
        }

        [HttpPost()]
        public async Task<ActionResult> AddBook([FromBody] Livro livro)
        {
            await _dataContext.Livro.AddAsync(livro);
            await _dataContext.SaveChangesAsync();
            return Ok(livro);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var livro = await _dataContext.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            _dataContext.Livro.Remove(livro);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Livro livro)
        {
            var existingLivro = await _dataContext.Livro.FindAsync(livro.LivroId);
            if (existingLivro == null)
            {
                return BadRequest("Livro não encontrado");
            }

            existingLivro.Titulo = livro.Titulo;
            existingLivro.Descricao = livro.Descricao;
            existingLivro.Autor = livro.Autor;

            _dataContext.SaveChanges();
            return Ok(existingLivro);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLivro(
            int id,
            [FromBody] JsonPatchDocument<Livro> livro
        )
        {
            if (livro == null)
            {
                return BadRequest();
            }

            var livrobyid = await _dataContext.Livro.FindAsync(id);

            if (livrobyid == null)
            {
                return NotFound();
            }

            livro.ApplyTo(livrobyid, ModelState);

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
                if (!LivroExists(id))
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

        private bool LivroExists(int id)
        {
            return _dataContext.Livro.Any(e => e.LivroId == id);
        }
    }
}
