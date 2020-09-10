using Quests.Server.Paging;
using Quests.Shared.Entities.Models;
using Quests.Shared.Entities.RequestFeatures;
using System.Threading.Tasks;

namespace Quests.Server.Repository
{
    public interface IQuestRepository
    {
        Task<PagedList<Quest>> GetQuests(QuestParameters questParameters);
        Task<PagedList<QuestStep>> GetQuestSteps(QuestParameters questParameters);
    }
}
