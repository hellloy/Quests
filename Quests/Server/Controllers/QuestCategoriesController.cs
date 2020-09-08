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
    public class QuestCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/QuestCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestCategory>>> GetQuestCategories()
        {
            return await _context.QuestCategories.ToListAsync();
        }

        // GET: api/QuestCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestCategory>> GetQuestCategory(int id)
        {
            var questCategory = await _context.QuestCategories.FindAsync(id);

            if (questCategory == null)
            {
                return NotFound();
            }

            return questCategory;
        }

        // PUT: api/QuestCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestCategory(int id, QuestCategory questCategory)
        {
            if (id != questCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(questCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestCategoryExists(id))
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

        // POST: api/QuestCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuestCategory>> PostQuestCategory(QuestCategory questCategory)
        {
            _context.QuestCategories.Add(questCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestCategory", new { id = questCategory.Id }, questCategory);
        }

        // DELETE: api/QuestCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestCategory>> DeleteQuestCategory(int id)
        {
            var questCategory = await _context.QuestCategories.Include("Quests").FirstOrDefaultAsync(x=>x.Id == id);
            
            if (questCategory == null)
            {
                return NotFound();
            }

            if (questCategory.Quests.Count > 0)
            {
                return Conflict();
            }

            _context.QuestCategories.Remove(questCategory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool QuestCategoryExists(int id)
        {
            return _context.QuestCategories.Any(e => e.Id == id);
        }
        
    }
}
