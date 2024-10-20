using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Data;
using DMS.Models;

namespace DMS.Controllers
{
    [ApiController]
    [Route("api/[controller]"), ]
    public class ClientController : ControllerBase
    {
        private readonly DmsDbContext _context;

        public ClientController(DmsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Client>>> GetClients([FromQuery]string clientid)
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()

        {
            try
            {
                var clients = await _context.Clients.ToListAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<Client>> PostClient(Client client)
        //{
        //    if (client == null)
        //    {
        //        return BadRequest("Client cannot be null.");
        //    }

        //    _context.Clients.Add(client);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetClients), new { id = client.Id }, client);
        //}

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            if (client == null)
            {
                return BadRequest("Client cannot be null.");
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var responseClient = new Client
            {
                fname = client.fname,
                lname = client.lname,
 
            };

            return CreatedAtAction(nameof(GetClients), responseClient);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.id)
            {
                return BadRequest("Client ID mismatch.");
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ClientExists(id))
                {
                    return NotFound("Client not found.");
                }
                else
                {
                    return StatusCode(500, "Database concurrency error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id)
        ;

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            _context.Clients.Remove(client);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok("Client deleted successfully.");
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.id == id);
        }


    }


}
