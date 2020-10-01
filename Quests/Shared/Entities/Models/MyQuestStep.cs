using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quests.Shared.Entities.Models
{
    public class MyQuestStep
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int MyQuestId { get; set; }
        [Required]
        public int QuestStepId { get; set; }
        [Required] 
        public MyQuestStepStatus Status { get; set; } = MyQuestStepStatus.NotStarted;
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Answer { get; set; }
        public bool Hint { get; set; } = false;
        public int Points { get; set; } = 0;

    }

    public enum MyQuestStepStatus
    {
        NotStarted = 0,
        Started = 1,
        Finished = 2,
    }
}
