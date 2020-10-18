using Quests.Shared.Entities.Models;

namespace Quests.Shared.VM
{
    public class MyQuestsVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public int Points { get; set; }
        public int StageCount { get; set; }
        public int CurrentProgress { get; set; }
        public int QuestId { get; set; }
        public MyQuestStatus Status { get; set; }
    }
}