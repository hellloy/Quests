using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Shared.Entities.Models;
using Quests.Shared.VM;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyQuestStepsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MyQuestStepsController(ApplicationDbContext context)
        {
            _context = context;
        }

       

        // GET: api/MyQuestSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetHint(int id)
        {
            var myQuestStep = await _context.MyQuestSteps.FindAsync(id);
            if (myQuestStep == null)
            {
                return NotFound();
            }
            var questStep = await _context.QuestSteps.FindAsync(myQuestStep.QuestStepId);

            myQuestStep.Hint = true;
            _context.Entry(myQuestStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyQuestStepExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return questStep.Hint;
        }

        
        [HttpPost]
        public async Task<ActionResult<AnswerVM>> PostMyQuestStep(AnswerVM answerVm)
        {
            if (answerVm == null || answerVm.MyQuestStepId == 0)
            {
                return BadRequest();
            }

            var myQuestStep = await _context.MyQuestSteps.FindAsync(answerVm.MyQuestStepId);
            if (myQuestStep == null)
            {
                return NotFound();
            }

            
            var questStep = await _context.QuestSteps.FindAsync(myQuestStep.QuestStepId);
            if (questStep == null)
            {
                return NotFound();
            }

            if (!string.Equals(answerVm.Message, questStep.Answer, StringComparison.CurrentCultureIgnoreCase))
            {
                answerVm.Result = false;
                answerVm.Message = "Ответ неправильный, попробуйте снова...";
                return answerVm;
            }

            var myQuestSteps = await _context.MyQuestSteps.Where(x=>x.MyQuestId == myQuestStep.MyQuestId).ToListAsync();
            var count = myQuestSteps.Count(x =>
                x.Status == MyQuestStepStatus.Started || x.Status == MyQuestStepStatus.NotStarted);

            answerVm.Result = true;
            myQuestStep.FinishDate = DateTime.Now;
            
            if (myQuestStep.Hint)
            {
                myQuestStep.Points = questStep.Points / 2;
                
            }
            else
            {
                myQuestStep.Points = questStep.Points;
            }

            

            if (count > 1)
            {
                answerVm.Message = $"Поздравляем Вы прошли текущий этап! и заработали {myQuestStep.Points} баллов";
            }
            else
            {
                var sum = myQuestSteps.Where(x=>x.Id != myQuestStep.Id).Sum(x => x.Points) + myQuestStep.Points;
                answerVm.Message =
                    $"Поздравляем, вы прошли квест! Ваше вознаграждение {sum} баллов. Используйте эти баллы для открытия других квестов";
                answerVm.Finish = true;
                var myQuest = await _context.MyQuests.FindAsync(myQuestStep.MyQuestId);
                myQuest.Status = MyQuestStatus.Finished;
                _context.Entry(myQuest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            myQuestStep.Status = MyQuestStepStatus.Finished;
            _context.Entry(myQuestStep).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return answerVm;
        }

        

        private bool MyQuestStepExists(int id)
        {
            return _context.MyQuestSteps.Any(e => e.Id == id);
        }
    }
}
