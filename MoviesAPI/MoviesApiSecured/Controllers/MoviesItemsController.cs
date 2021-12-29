using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesApi.Models;
using TodoApi.Attributes;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class MoviesItemsController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesItemsController(MoviesContext context)
        {
            _context = context;
        }



        // GET: api/MoviesItems   notice: we don't use method name
        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation("Zwraca wszystkie zadania.", "Używa EF")]
        [SwaggerResponse(200, "Sukces", Type = typeof(List<MoviesItem>))]
        
        public async Task<ActionResult<IEnumerable<MoviesItem>>> GetMoviesItems()
        {
            return await _context.MoviesItems.ToListAsync();  //http 200
        }



        // GET: api/MoviesItems/5
        [HttpGet("{id}")]        
        [Produces("application/json")]
        [SwaggerOperation("Zwraca zadanie o podanym {id}.", "Używa EF FindAsync()")]
        [SwaggerResponse(200, "Znaleziono zadanie o podanym {id}", Type = typeof(MoviesItem))]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<ActionResult<MoviesItem>> GetMoviesItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz odczytać", Required = true)]
            int id)
        {
            var moviesItem = await _context.MoviesItems.FindAsync(id);

            if (moviesItem == null)
            {
                return NotFound(); //http 404
            }

            return moviesItem;    //http 200
        }


        // PUT: api/MoviesItems/5        
        [HttpPut("{id}")]
        
        [Produces("application/json")]
        [SwaggerOperation("Aktualizuje zadanie o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Zaktualizowano zadanie o podanym {id}")]
        [SwaggerResponse(400, "Nie rozpoznano danych wejściowych")]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<IActionResult> PutMoviesItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz zaktualizować", Required = true)]
            int id,
            [SwaggerParameter("Definicja zadania", Required = true)]
            MoviesItem moviesItem)
        {
            if (id != moviesItem.Id)
            {
                return BadRequest(); //http 400
            }

            _context.Entry(moviesItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoviesItemExists(id))
                {
                    return NotFound();  //http 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //http 204
        }


        // POST: api/MoviesItems        
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation("Tworzy nowe zadanie.", "Używa EF")]
        [SwaggerResponse(201, "Zapis z sukcesem.", Type = typeof(MoviesItem))]
        public async Task<ActionResult<MoviesItem>> PostMoviesItem(
            [SwaggerParameter("Definicja zadania", Required = true)]
            MoviesItem moviesItem)
        {
            _context.MoviesItems.Add(moviesItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMoviesItem", new { id = moviesItem.Id }, moviesItem);  //http 201, add Location header
        }



        // DELETE: api/MoviesItems/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Usuwa zadanie o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Usunięto zadanie o podanym {id}")]        
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<IActionResult> DeleteMoviesItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz usunąć", Required = true)]
            int id)
        {
            var moviesItem = await _context.MoviesItems.FindAsync(id);
            if (moviesItem == null)
            {
                return NotFound();  //http 404
            }

            _context.MoviesItems.Remove(moviesItem);
            await _context.SaveChangesAsync();

            return NoContent(); //http 204
        }



        private bool MoviesItemExists(int id)
        {
            return _context.MoviesItems.Any(e => e.Id == id);
        }
    }
}
