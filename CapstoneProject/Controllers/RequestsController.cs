using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Models;

namespace CapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context) {
            _context = context;
        }

        //review method
        [HttpPut("Review/{id}")]
        public async Task<ActionResult> reviewRequest(int id, Request request) {
            if (_context.Requests == null) {
                return BadRequest();
            }

            if (request.Total <= 50) {
                request.Status = "Approved";
            } else {
                request.Status = "Review";
            }

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //aprove method
        [HttpPut("Approve/{id}")]
        public async Task<ActionResult> ApproveRequest(int id, Request request) {
            if (_context.Requests == null) {
                return BadRequest();
            }
            request.Status = "Approved";

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //reject method
        [HttpPut("Reject/{id}")]
        public async Task<ActionResult> RejectRequest(int id, Request request) {
            if (_context.Requests == null) {
                return BadRequest();
            }
            request.Status = "Rejected";

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // return all requests under review that arent linked to logged in user
        [HttpGet("GetReviews/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetAllReview(int id) {
            if (_context.Requests == null) {
                return NotFound();
            }

            var req = new List<Request>();
            foreach (var i in _context.Requests) {
                if(i.UserId != id && i.Status == "Review") {
                    req.Add(i);
                }
            }
            return req;

        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            return await _context.Requests.Include(x => x.User).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            var request = await _context.Requests
                                        .Include(x => x.User)
                                        .Include(x => x.RequestLine)!
                                            .ThenInclude(x => x.Product)
                                        .SingleOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            //Request req = await _context.Requests.FindAsync(id);

            //// user not able to change status or total
            //if(request.Status != req.Status || request.Total != req.Total) {
            //    return BadRequest();
            //}

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
          if (_context.Requests == null)
          {
              return Problem("Entity set 'AppDbContext.Requests'  is null.");
          }

          // assign total based on requestline quantity and product price
          // THINK THIS IS TAKEN CARE OF USING RECALCULATE IN THE REQUESTLINESCONTROLLER
          //request.Total = (from rl in _context.RequestLines
          //                 join p in _context.Products
          //                      on rl.ProductId equals p.Id
          //                  where rl.RequestId == request.Id
          //                  select new {
          //                      LineTotal = p.Price * rl.Quantity
          //                  }).Sum(x => x.LineTotal);

            // status defaults to New
            request.Status = "New";
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
