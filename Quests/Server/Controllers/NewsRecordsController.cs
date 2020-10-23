using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Shared.Entities.Models;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NewsRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/NewsRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsRecord>>> GetNewsRecords()
        {
            return await _context.NewsRecords.ToListAsync();
        }

        // GET: api/NewsRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsRecord>> GetNewsRecord(int id)
        {
            var newsRecord = await _context.NewsRecords.FindAsync(id);

            if (newsRecord == null)
            {
                return NotFound();
            }

            return newsRecord;
        }

        // PUT: api/NewsRecords/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsRecord(int id, NewsRecord newsRecord)
        {
            if (id != newsRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(newsRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsRecordExists(id))
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

        // POST: api/NewsRecords
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NewsRecord>> PostNewsRecord(NewsRecord newsRecord)
        {
            _context.NewsRecords.Add(newsRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNewsRecord", new { id = newsRecord.Id }, newsRecord);
        }

        // DELETE: api/NewsRecords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NewsRecord>> DeleteNewsRecord(int id)
        {
            var newsRecord = await _context.NewsRecords.FindAsync(id);
            if (newsRecord == null)
            {
                return NotFound();
            }

            _context.NewsRecords.Remove(newsRecord);
            await _context.SaveChangesAsync();

            return newsRecord;
        }

        private bool NewsRecordExists(int id)
        {
            return _context.NewsRecords.Any(e => e.Id == id);
        }
    }
}
