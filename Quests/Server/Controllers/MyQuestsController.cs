using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Server.Models;
using Quests.Shared.Entities.Models;
using Quests.Shared.VM;

namespace Quests.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class MyQuestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyQuestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: api/MyQuests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyQuestsVm>>> GetMyQuests()
        {
            var currentUserId = _userManager.GetUserId(User);
            var myQuests = await _context.MyQuests.Include(x => x.Quest).Where(x => x.UserId == currentUserId).ToListAsync();
            var myQuestSteps = await _context.MyQuestSteps.Where(x => myQuests.Select(y => y.Id).Contains(x.MyQuestId)).ToListAsync();
            
            
            List<MyQuestsVm> myQuestsVms = new List<MyQuestsVm>();
            foreach (var item in myQuests)
            {
                var steps = myQuestSteps.Where(x => x.MyQuestId == item.Id).ToList();
                MyQuestsVm myQuestsVm = new MyQuestsVm
                {
                    Status = item.Status,
                    Points = steps.Sum(x => x.Points),
                    Name = item.Quest.Name,
                    Id = item.Id,
                    StageCount = steps.Count(x => x.MyQuestId == item.Id),
                    CurrentProgress = GetCurrentProgress(steps),
                    Img = item.Quest.Img,
                    QuestId = item.QuestId
                };
                myQuestsVms.Add(myQuestsVm);
            }
            return myQuestsVms;
        }

        // GET: api/MyQuests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MyQuestVm>> GetMyQuest(int id)
        {
            var currentUserId = _userManager.GetUserId(User);

            var myQuest = await _context.MyQuests.Include(x => x.Quest).FirstOrDefaultAsync(x => x.Id == id &&
                x.UserId == currentUserId &&
                x.Status != MyQuestStatus.Finished);
            if (myQuest == null)
            {
                return NotFound();
            }

            var myQuestStep = _context.MyQuestSteps.FirstOrDefault(x => x.MyQuestId == myQuest.Id &&
                                                                        x.Status != MyQuestStepStatus.Finished);
            if (myQuestStep == null)
            {
                return NotFound();
            }

            if (myQuest.Status == MyQuestStatus.NotStarted)
            {
                myQuest.Status = MyQuestStatus.Started;
                _context.Entry(myQuest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            if (myQuestStep.Status == MyQuestStepStatus.NotStarted)
            {
                myQuestStep.Status = MyQuestStepStatus.Started;
                myQuestStep.StartDate = DateTime.Now;
                _context.Entry(myQuestStep).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var vm = new MyQuestVm
            {
                MyQuest = myQuest,
                MyActiveStep = myQuestStep,
                QuestStep = await _context.QuestSteps.FindAsync(myQuestStep.QuestStepId)
            };
            

            return vm;
        }

        // PUT: api/MyQuests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyQuest(int id, MyQuest myQuest)
        {
            if (id != myQuest.Id)
            {
                return BadRequest();
            }

            _context.Entry(myQuest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyQuestExists(id))
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

        // POST: api/MyQuests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MyQuest>> PostMyQuest(Quest quest)
        {

            var dbQuest = await _context.Quests.FindAsync(quest.Id);
            var currentUserId = _userManager.GetUserId(User);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            if (dbQuest.Price > user.Points)
            {
                return NotFound();
            }

            var existsQuest = await _context.MyQuests.FirstOrDefaultAsync(x => x.UserId == currentUserId && x.QuestId == quest.Id);

            if (existsQuest == null)
            {
                var questSteps = _context.QuestSteps.Where(x => x.QuestId == quest.Id);
                var myQuest = new MyQuest
                {
                    QuestId = quest.Id,
                    Status = MyQuestStatus.NotStarted,
                    PurchaseDateTime = DateTime.Now,
                    UserId = currentUserId

                };


                await _context.MyQuests.AddAsync(myQuest);
                await _context.SaveChangesAsync();

                List<MyQuestStep> myQuestSteps = new List<MyQuestStep>();
                foreach (var step in questSteps.OrderBy(x=>x.StepNumber))
                {
                    var myQuestStep = new MyQuestStep
                    {
                        MyQuestId = myQuest.Id,
                        QuestStepId = step.Id,
                        Status = MyQuestStepStatus.NotStarted
                    };
                    myQuestSteps.Add(myQuestStep);
                }

                await _context.MyQuestSteps.AddRangeAsync(myQuestSteps);
                await _context.SaveChangesAsync();

                user.Points -= quest.Price;
                await _userManager.UpdateAsync(user);

                return CreatedAtAction("GetMyQuest", new { id = myQuest.Id }, myQuest);
            }

            return existsQuest;

        }

        // DELETE: api/MyQuests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MyQuest>> DeleteMyQuest(int id)
        {
            var myQuest = await _context.MyQuests.FindAsync(id);
            if (myQuest == null)
            {
                return NotFound();
            }

            _context.MyQuests.Remove(myQuest);
            await _context.SaveChangesAsync();

            return myQuest;
        }

        private bool MyQuestExists(int id)
        {
            return _context.MyQuests.Any(e => e.Id == id);
        }

        private int GetCurrentProgress(List<MyQuestStep> myQuestSteps)
        {
            
            int count = myQuestSteps.Count;
            int finished = myQuestSteps.Count(x => x.Status == MyQuestStepStatus.Finished);
            if (count == 0 || finished == 0)
            {
                return 0;
            }
            var result = ((double)finished/count) * 100;
            return (int)result;
        }
    }
}
