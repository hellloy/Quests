using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Server.Paging;
using Quests.Shared.Entities.Models;
using System.Threading.Tasks;
using Quests.Server.Repository.RepositoryExtensions;
using Quests.Shared.Entities.RequestFeatures;

namespace Quests.Server.Repository
{
    public class QuestRepository : IQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Quest>> GetQuests(QuestParameters questParameters)
        {
            var quests = await _context.Quests
                .Search(questParameters.SearchTerm)
                .Sort(questParameters.OrderBy)
                .ToListAsync();
            return PagedList<Quest>.ToPagedList(quests, questParameters.PageNumber, questParameters.PageSize);
        }
        public async Task<PagedList<QuestStep>> GetQuestSteps(QuestParameters questParameters)
        {
            var questSteps = await _context.QuestSteps.Include(x=>x.Quest)
                .Search(questParameters.SearchTerm)
                .Sort(questParameters.OrderBy)
                .ToListAsync();
            return PagedList<QuestStep>.ToPagedList(questSteps, questParameters.PageNumber, questParameters.PageSize);
        }

    }
}
