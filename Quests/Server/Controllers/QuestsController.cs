using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quests.Server.Data;
using Quests.Server.Repository;
using Quests.Shared.Entities.Models;
using Quests.Shared.Entities.RequestFeatures;
using System.Linq;
using System.Threading.Tasks;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestRepository _repo;
        

        public QuestsController(ApplicationDbContext context, IQuestRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/Quests
        [HttpGet]
        public async Task<ActionResult> GetQuests([FromQuery] QuestParameters questParameters,bool all)
        {
            if (all) return Ok(await _context.Quests.ToListAsync());
            var quests = await _repo.GetQuests(questParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(quests.MetaData));
            return Ok(quests);
        }

       
        // GET: api/Quests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quest>> GetQuest(int id)
        {
            var quest = await _context.Quests.FindAsync(id);

            if (quest == null)
            {
                return NotFound();
            }

            return quest;
        }

        // PUT: api/Quests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuest(int id, Quest quest)
        {
            if (id != quest.Id)
            {
                return BadRequest();
            }

            _context.Entry(quest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestExists(id))
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

        // POST: api/Quests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Quest>> PostQuest(Quest quest)
        {
            _context.Quests.Add(quest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuest", new { id = quest.Id }, quest);
        }

        // DELETE: api/Quests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Quest>> DeleteQuest(int id)
        {
            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            _context.Quests.Remove(quest);
            await _context.SaveChangesAsync();

            return quest;
        }


        private bool QuestExists(int id)
        {
            return _context.Quests.Any(e => e.Id == id);
        }
    }
}
