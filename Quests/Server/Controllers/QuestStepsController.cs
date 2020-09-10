using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quests.Server.Data;
using Quests.Server.Repository;
using Quests.Shared.Entities.Models;
using Quests.Shared.Entities.RequestFeatures;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestStepsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestRepository _repo;

        public QuestStepsController(ApplicationDbContext context, IQuestRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/QuestSteps
        [HttpGet]
        public async Task<ActionResult> GetQuestSteps([FromQuery] QuestParameters questParameters,bool all)
        {
            if (all) return Ok(await _context.QuestSteps.ToListAsync());

            var questSteps = await _repo.GetQuestSteps(questParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(questSteps.MetaData));
            return Ok(questSteps);
            
        }
        


        // GET: api/QuestSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestStep>> GetQuestStep(int id)
        {
            var questStep = await _context.QuestSteps.FindAsync(id);

            if (questStep == null)
            {
                return NotFound();
            }

            return questStep;
        }

        // PUT: api/QuestSteps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestStep(int id, QuestStep questStep)
        {
            if (id != questStep.Id)
            {
                return BadRequest();
            }

            _context.Entry(questStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestStepExists(id))
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

        // POST: api/QuestSteps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuestStep>> PostQuestStep(QuestStep questStep)
        {
            _context.QuestSteps.Add(questStep);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestStep", new { id = questStep.Id }, questStep);
        }

        // DELETE: api/QuestSteps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestStep>> DeleteQuestStep(int id)
        {
            var questStep = await _context.QuestSteps.FindAsync(id);
            if (questStep == null)
            {
                return NotFound();
            }

            _context.QuestSteps.Remove(questStep);
            await _context.SaveChangesAsync();

            return questStep;
        }

        private bool QuestStepExists(int id)
        {
            return _context.QuestSteps.Any(e => e.Id == id);
        }
    }
}
