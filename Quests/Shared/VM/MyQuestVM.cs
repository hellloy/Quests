using Quests.Shared.Entities.Models;

namespace Quests.Shared.VM
{
    public class MyQuestVm
    {
        public MyQuest MyQuest { get; set; }
        public MyQuestStep MyActiveStep { get; set; }
        public QuestStep QuestStep { get; set; }

    }
}